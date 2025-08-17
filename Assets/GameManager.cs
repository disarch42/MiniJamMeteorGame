using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    static private GameManager _current;
    static public GameManager GetInstance() { return _current; }

    [HideInInspector] public List<Meteor> meteors;

    [HideInInspector] public List<Collectible> availableCollectibles;
    [HideInInspector] public List<Collectible> cachedCollectibles;

    [Header("meteor collision")]
    public Bounds rectBounds;
    [Range(0.0f, 2.0f)]
    public float collisionVelocityMult;

    [Header("references")]
    //taking references like this might be messy
    public Image cursorImage;
    public Transform cursorCanvas;
    public GameObject cursorSliderGameObject;
    public Slider cursorSlider;
    public Image cursorSliderFillImage;
    public GameObject collectiblePrefab;
    public GameObject shockwavePrefab;
    public MeteorExplosionParticle meteorExplosionParticle;
    public Volume postProcessVolume;

    [Header("black hole")]
    public float maxBlackHoleRadius;
    public float minBlackHoleRadius;
    public float minChargeTime;
    public float maxChargeTime;
    public float blackHoleFreezeTime;
    private float _chargeTime = -1;
    private float _lastFrameTime = -1;
    public float blackHoleMinSpeed;
    
    public BlackHoleRing blackHolePreviewRing;
    //set chargetime to -1 if we are not charging 
    private bool charging { get { return _chargeTime >= 0; } }

    [Header("post process")]
    public float paniniIdle;
    public float paniniCharged;
    public float lensDistortionIdle;
    public float lensDistortionCharged;
    public float chromaticAberrIdle;
    public float chromaticAberCharged;
    public float afterEffectsTime;
    private float _afterEffectsTimer;

    [Header("other")]
    public float collectibleMouseCollectRadius = 0.2f;
    //!!! if something else changes timescale will need to rewrite this
    public float chargingTimeScale = 0.5f;

    private bool _holdingM1;
    private Vector2 _mouseScreenPos;
    private Vector2 _mouseWorldPos;
    public void SetValuesFromStats()
    {
        maxBlackHoleRadius = StatsManager.instance.maxBlackHoleRadius;
        minBlackHoleRadius = StatsManager.instance.minBlackHoleRadius;
        minChargeTime = StatsManager.instance.minBlackholeChargeTime;
        maxChargeTime = StatsManager.instance.maxBlackholeChargeTime;
        blackHoleFreezeTime = StatsManager.instance.blackHoleFreezeTime;
        collectibleMouseCollectRadius = StatsManager.instance.mouseCollectRadius;
        chargingTimeScale = StatsManager.instance.chargingTimeScale;        
    }
    public void CreateCollectibles(Vector2 point, float randomRange, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Collectible c;
            if (cachedCollectibles.Count > 0) 
            { 
                c = cachedCollectibles[0]; 
                cachedCollectibles.RemoveAt(0); 
            }
            else
            {
                c = GameObject.Instantiate(collectiblePrefab).GetComponent<Collectible>();
            }
            c.gameObject.SetActive(true);
            c.transform.position = point + Random.insideUnitCircle*Random.Range(0.0f,randomRange);
            c.InitializeCollectible(5);
            availableCollectibles.Add(c);
        }
    }
    private void CreateShockwave(Vector2 pos, float r)
    {
        Instantiate(shockwavePrefab).GetComponent<Shockwave>().Initialize(pos, blackHoleFreezeTime*1.25f, r);
    }
    public void CreateExplosionEffect(float r, Vector2 p, Vector2 initialSpeed)
    {
        meteorExplosionParticle.Explode(r, p, initialSpeed);
    }
    private void Awake()
    {
        _current = this;
    }
    private void Start()
    {
        SetValuesFromStats();
        Cursor.visible = false;
        blackHolePreviewRing.gameObject.SetActive(false);

        InputSystem.GetCurrent().actions.Player.MousePosition.performed += ctx =>
        {
            cursorImage.enabled = true;
            _mouseScreenPos = ctx.ReadValue<Vector2>();
            _mouseWorldPos = Camera.main.ScreenToWorldPoint(_mouseScreenPos);
        };
        InputSystem.GetCurrent().actions.Player.MousePosition.canceled += ctx =>
        {
            cursorImage.enabled = false;
            _mouseScreenPos = Vector2.zero;
            _mouseWorldPos = Vector2.zero;
        };

        InputSystem.GetCurrent().actions.Player.BlackHole.performed += ctx => { _holdingM1 = true; };
        InputSystem.GetCurrent().actions.Player.BlackHole.canceled += ctx => { _holdingM1 = false; };

        cursorSliderGameObject.SetActive(false);
    }
    private void Update()
    {
        cursorCanvas.position = _mouseWorldPos;
        if (charging)
        {
            float updChargeTime = _chargeTime + (Time.time - _lastFrameTime);
            float t = Mathf.InverseLerp(0, maxChargeTime, updChargeTime);
            cursorSlider.value = t;
            cursorSliderFillImage.color = updChargeTime > minChargeTime ? Color.blueViolet : Color.indianRed;
            blackHolePreviewRing.SetRadius(Mathf.Lerp(minBlackHoleRadius, maxBlackHoleRadius, t));
            blackHolePreviewRing.transform.position = _mouseWorldPos;
            SetPostProcesses(t/1.5f);
            float currentRadius = Mathf.Lerp(minBlackHoleRadius, maxBlackHoleRadius, Mathf.InverseLerp(0, maxChargeTime, updChargeTime));
            //arrow preview
            for (int i = 0; i < meteors.Count; i++)
            {
                Vector2 distanceVector = _mouseWorldPos - (Vector2)meteors[i].spriteRendererTransform.position;
                float dist = distanceVector.magnitude - meteors[i].radius;
                meteors[i].arrowTransform.gameObject.SetActive(dist < currentRadius);

                Vector2 newDir = distanceVector.normalized;

                meteors[i].arrowTransform.eulerAngles = new Vector3(meteors[i].arrowTransform.eulerAngles.x, meteors[i].arrowTransform.eulerAngles.y,
                    Mathf.Rad2Deg * Mathf.Atan2(newDir.y, newDir.x) - 90);

                meteors[i].arrowTransform.position = meteors[i].spriteRendererTransform.position + (Vector3)(newDir.normalized * meteors[i].radius);
            }
        }
        else if(_afterEffectsTimer>0)
        {
            _afterEffectsTimer -= Time.deltaTime;
            SetPostProcesses(_afterEffectsTimer / afterEffectsTime);
        }
    }
    public void SetPostProcesses(float t)
    {
        if (postProcessVolume.profile.TryGet<PaniniProjection>(out PaniniProjection pan))
        {
            pan.distance.Override(Mathf.Lerp(paniniIdle, paniniCharged, t));
        }
        if (postProcessVolume.profile.TryGet<LensDistortion>(out LensDistortion lens))
        {
            lens.intensity.Override(Mathf.Lerp(paniniIdle, paniniCharged, t));
        }
        if (postProcessVolume.profile.TryGet<ChromaticAberration>(out ChromaticAberration aber))
        {
            aber.intensity.Override(Mathf.Lerp(chromaticAberrIdle, chromaticAberCharged, t));
        }
    }
    private void FixedUpdate()
    {
        CheckMeteorCollisions();
        BlackHoleUpdate();
        _lastFrameTime = Time.time;
        void CheckMeteorCollisions()
        {
            for (int i = 0; i < meteors.Count; i++)
            {
                for (int j = i + 1; j < meteors.Count; j++)
                {
                    float dist = Vector2.Distance(meteors[i].transform.position, meteors[j].transform.position);
                    if (dist < (meteors[i].radius + meteors[j].radius))
                    {
                        Meteor meteorA = meteors[i];
                        Meteor meteorB = meteors[j];

                        Vector2 collisionNormal = ((Vector2)meteorB.transform.position - (Vector2)meteorA.transform.position).normalized;
                        Vector2 collisionTangent = new Vector2(-collisionNormal.y, collisionNormal.x);

                        float meteorATangent = Vector2.Dot(meteorA.velocity, collisionTangent);
                        float meteorBTangent = Vector2.Dot(meteorB.velocity, collisionTangent);

                        //these swap
                        float meteorANormal = Vector2.Dot(meteorA.velocity, collisionNormal);
                        float meteorBNormal = Vector2.Dot(meteorB.velocity, collisionNormal);

                        float meteorANormalPrev = meteorANormal;

                        meteorANormal = (meteorANormal * (meteorA.mass - meteorB.mass) + 2 * meteorB.mass * meteorBNormal) / (meteorA.mass + meteorB.mass);
                        meteorBNormal = (meteorBNormal * (meteorB.mass - meteorA.mass) + 2 * meteorA.mass * meteorANormalPrev) / (meteorA.mass + meteorB.mass);

                        meteorA.velocity = meteorANormal * collisionNormal + meteorATangent * collisionTangent;
                        meteorB.velocity = meteorBNormal * collisionNormal + meteorBTangent * collisionTangent;

                        meteorA.velocity *= collisionVelocityMult;
                        meteorB.velocity *= collisionVelocityMult;

                        float p = meteorA.radius + meteorB.radius - dist;
                        Vector2 meteorApos = meteorA.transform.position;
                        meteorApos -= (p * (1.0f / meteorA.mass) * collisionNormal) / ((1.0f / meteorA.mass) + (1.0f / meteorB.mass));

                        Vector2 meteorBpos = meteorB.transform.position;
                        meteorBpos += (p * (1.0f / meteorB.mass) * collisionNormal) / ((1.0f / meteorA.mass) + (1.0f / meteorB.mass));

                        meteorA.transform.position = new Vector3(meteorApos.x, meteorApos.y, meteorA.transform.position.z);
                        meteorB.transform.position = new Vector3(meteorBpos.x, meteorBpos.y, meteorB.transform.position.z);

                        float meteorADmg = meteorA.damage;
                        float meteorBDmg = meteorB.damage;

                        meteorA.DamageMeteor(meteorBDmg);


                        meteorB.DamageMeteor(meteorADmg);

                        ScreenShake.instance.AddScreenShake(0.1f, 0.15f);
                    }
                }
            }
        }
        void BlackHoleUpdate()
        {
            //just started charging
            if (!charging && _holdingM1)
            {
                _chargeTime = 0;
                blackHolePreviewRing.gameObject.SetActive(true);
                cursorSliderGameObject.SetActive(true);
            }

            //collect collectibles with mouse
            if (!charging)
            {
                for (int i = availableCollectibles.Count - 1; i >= 0; i--)
                {
                    if (Vector2.Distance(availableCollectibles[i].transform.position, _mouseWorldPos) < collectibleMouseCollectRadius)
                    {
                        availableCollectibles[i].OnCollect(_mouseWorldPos, 0.0f);
                        availableCollectibles.RemoveAt(i);
                    }
                }
            }

            if (charging)
            {
                _chargeTime += Time.fixedDeltaTime;
                float t = Mathf.InverseLerp(0, maxChargeTime, _chargeTime);
                Time.timeScale = Mathf.Lerp(1.0f, chargingTimeScale, t);
                float currentRadius = Mathf.Lerp(minBlackHoleRadius, maxBlackHoleRadius, t);
                blackHolePreviewRing.SetRadius(currentRadius);
                blackHolePreviewRing.transform.position = _mouseWorldPos;
                //end charge
                if (!_holdingM1)
                {
                    _afterEffectsTimer = afterEffectsTime;
                    Time.timeScale = 1.0f;
                    blackHolePreviewRing.gameObject.SetActive(false);
                    ScreenShake.instance.AddScreenShake(0.1f, 0.08f);

                    CreateShockwave(_mouseWorldPos, currentRadius);
                    foreach (Meteor meteor in meteors)
                    {
                        meteor.arrowTransform.gameObject.SetActive(false);
                    }
                    if (_chargeTime > minChargeTime)
                    {
                        for (int i = 0; i < meteors.Count; i++)
                        {
                            Vector2 distanceVector = _mouseWorldPos - (Vector2)meteors[i].transform.position;
                            float dist = distanceVector.magnitude-meteors[i].radius;
                            if (dist < currentRadius)
                            {
                                Vector2 newDir = distanceVector.normalized;
                                meteors[i].ChangeVelocity(blackHoleFreezeTime, newDir * Mathf.Max(meteors[i].velocity.magnitude, blackHoleMinSpeed));
                            }
                        }
                    }

                    for (int i = availableCollectibles.Count - 1; i >= 0; i--)
                    {
                        if (Vector2.Distance(availableCollectibles[i].transform.position, _mouseWorldPos) < currentRadius)
                        {
                            availableCollectibles[i].OnCollect(_mouseWorldPos, blackHoleFreezeTime);
                            availableCollectibles.RemoveAt(i);
                        }
                    }
                    cursorSliderGameObject.SetActive(false);
                    _chargeTime = -1;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(rectBounds.center, rectBounds.size);
        if (charging)
        {
            Gizmos.color = Color.purple;
            float currentRadius = Mathf.Lerp(minBlackHoleRadius, maxBlackHoleRadius, Mathf.InverseLerp(0, maxChargeTime, _chargeTime));
            Gizmos.DrawWireSphere(_mouseWorldPos, currentRadius);
        }
    }
}
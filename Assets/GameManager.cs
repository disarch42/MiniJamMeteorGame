using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    static private GameManager _current;
    static public GameManager GetInstance() { return _current; }

    [HideInInspector] public List<Meteor> meteors;

    [Header("meteor collision")]
    public Bounds rectBounds;
    [Range(0.0f, 2.0f)]
    public float collisionVelocityMult;

    [Header("black hole")]
    //taking references like this might be messy
    public Image cursorImage;
    public Transform cursorCanvas;
    public GameObject cursorSliderGameObject;
    public Slider cursorSlider;
    public Image cursorSliderFillImage;

    public float maxBlackHoleRadius;
    public float minBlackHoleRadius;
    public float minChargeTime;
    public float maxChargeTime;
    public float freezeTime;

    private bool _holdingM1;
    private Vector2 _mouseScreenPos;
    private Vector2 _mouseWorldPos;
    private float _chargeTime = -1;
    //setting chargetime to -1 if we are not charging 
    private bool charging { get { return _chargeTime >= 0; } }

    private void Awake()
    {
        _current = this;
    }
    private void Start()
    {
        Cursor.visible = false;

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
            float currentRadius = Mathf.Lerp(minBlackHoleRadius, maxBlackHoleRadius, Mathf.InverseLerp(0, maxChargeTime, _chargeTime));
            //arrow preview
            for (int i = 0; i < meteors.Count; i++)
            {
                Vector2 distanceVector = _mouseWorldPos - (Vector2)meteors[i].transform.position;
                float dist = distanceVector.magnitude - meteors[i].radius;
                meteors[i].arrowTransform.gameObject.SetActive(dist < currentRadius);

                Vector2 newDir = distanceVector.normalized;

                meteors[i].arrowTransform.eulerAngles = new Vector3(meteors[i].arrowTransform.eulerAngles.x, meteors[i].arrowTransform.eulerAngles.y,
                    Mathf.Rad2Deg * Mathf.Atan2(newDir.y, newDir.x) - 90);

                meteors[i].arrowTransform.position = meteors[i].spriteRendererTransform.position + (Vector3)(newDir.normalized * meteors[i].radius);
            }
        }
    }
    private void FixedUpdate()
    {
        CheckMeteorCollisions();
        BlackHoleUpdate();
        void CheckMeteorCollisions()
        {
            for (int i = 0; i < meteors.Count; i++)
            {
                for (int j = i + 1; j < meteors.Count; j++)
                {
                    float dist = Vector2.Distance(meteors[i].transform.position, meteors[j].transform.position);
                    if (dist < meteors[i].radius + meteors[j].radius)
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
                cursorSliderGameObject.SetActive(true);
                foreach (Meteor meteor in meteors)
                {
                    meteor.arrowTransform.gameObject.SetActive(false);
                }
            }

            if (charging)
            {
                _chargeTime += Time.fixedDeltaTime;
                float t = Mathf.InverseLerp(0, maxChargeTime, _chargeTime);
                cursorSliderFillImage.color = _chargeTime > minChargeTime ? Color.blueViolet : Color.indianRed;
                cursorSlider.value = t;
                float currentRadius = Mathf.Lerp(minBlackHoleRadius, maxBlackHoleRadius, t);
                //end charge
                if (!_holdingM1)
                {
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
                                meteors[i].ChangeVelocity(freezeTime, newDir * meteors[i].velocity.magnitude);
                            }
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
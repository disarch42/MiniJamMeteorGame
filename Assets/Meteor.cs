using UnityEngine;

public class Meteor : MonoBehaviour
{
    [Header("references")]
    public Transform arrowTransform;
    public Transform spriteRendererTransform;
    private Material _spriteRendMaterial;
    private SpriteRenderer spriteRend;
    [Header("stats")]
    public Vector2 velocity;
    public float radius;
    public float mass;
    //health is determined by the hit of the weakest meteor.
    public float maxHealth;
    public float health;
    //probably something like 1/5 of health would be nice
    public float damage;
    public Gradient hpColor;
    public float hpColorGlow;
    //this is not used anywhere
    public float kineticEnergy;

    //[Header()]

    private int _totalCurrencyDrop;

    //this is for when we get destroyed
    private int _destroyDropCurrency;
    //this is for when we get damaged. 
    private int _leftDamageCurrency;

    //private
    private float _lastFrameTime;
    private Vector3 _prevPos;
    //if positive freeze
    private float _freezeTimer=-1;
    public const float maxSpeed = 30.0f;
    private int _remainingBounces;
    private bool _isDestroyed = false;

    public bool isBomb;
    public float explosionRadius;
    public float bombDamage;
    public GameObject shockwavePrefab;

    public Sprite crack0Sprite;
    public Sprite crack1Sprite;
    public Sprite crack2Sprite;

    public Transform trail;
    // since meteors will probably be cached having a function that resets all values will be nice
    public void InitializeMeteor(float _radius, int bounceAmount, float _mass, float _health, int _currencyDrop, float _damage, Vector2 _initialVelocity)
    {
        if (_radius == 0) { _radius = transform.localScale.x; }
        if (_mass == 0) { _mass = 1; }
        if (_health == 0) {  health = 1; }
        transform.localScale = Vector3.one * _radius * 2;
        radius = _radius;
        mass = _mass;
        velocity = _initialVelocity;
        _remainingBounces = bounceAmount;

        _totalCurrencyDrop = _currencyDrop;
        _leftDamageCurrency = (int)(_currencyDrop*0.2f);
        _destroyDropCurrency = _currencyDrop - _leftDamageCurrency;
        
        maxHealth = _health;
        health = _health;
        damage = _damage;
    }
    private void Start()
    {
        //ideally we only create meteors from meteorspawner so we dont want this anymore.
        //removing this will cause issues with meteors that are not created with spawner.
        //InitializeMeteor(radius, maxHealth, mass, currencyDrop, damage, velocity);
        spriteRend = spriteRendererTransform.GetComponent<SpriteRenderer>();
        spriteRend.sprite = crack0Sprite;
        _spriteRendMaterial = spriteRend.material;
        _spriteRendMaterial.SetColor("_EmissionColor", hpColor.Evaluate(health/maxHealth) * hpColorGlow);
    }
    private void OnEnable()
    {
        if (!GameManager.GetInstance().meteors.Contains(this))
        {
            GameManager.GetInstance().meteors.Add(this);
        }

    }
    private void OnDisable()
    {
        RemoveMeteor();
    }
    void RemoveMeteor()
    {
        _isDestroyed = true;
        if (GameManager.GetInstance().meteors.Contains(this))
        {
            GameManager.GetInstance().meteors.Remove(this);
        }
    }
    private void Update()
    {
        if( _isDestroyed) { return; }
        spriteRendererTransform.position = Vector3.Lerp(_prevPos, transform.position, (Time.time-_lastFrameTime) / Time.fixedDeltaTime);
        float rad = Mathf.Atan2(velocity.y, velocity.x);
        trail.transform.eulerAngles = new Vector3(0, 0, rad*Mathf.Rad2Deg+180);
        trail.transform.position = transform.position - new Vector3(Mathf.Cos(rad) * radius * 1.1f, Mathf.Sin(rad) * radius * 1.1f, 0);

    }
    public void DamageMeteor(float dmg)
    {
        if (_isDestroyed) { return; }

        health -= dmg;
        int dropAmount = Mathf.Min((int)Mathf.Ceil( _totalCurrencyDrop * (dmg/maxHealth)*0.2f ), _leftDamageCurrency);
        _leftDamageCurrency -= dropAmount;
        GameManager.GetInstance().CreateCollectibles(transform.position, radius, dropAmount);
        float t = health / maxHealth;
        if (spriteRend != null)
        {
            if (t < .33f) { spriteRend.sprite = crack2Sprite; }
            else if (t < .66f) { spriteRend.sprite = crack1Sprite; }
            else { spriteRend.sprite = crack0Sprite; }
        }

        if (_spriteRendMaterial != null)
        {
            _spriteRendMaterial.SetColor("_EmissionColor", hpColor.Evaluate(health / maxHealth) * hpColorGlow);
        }
        if (health < 0)
        {
            _isDestroyed = true;
            GameManager.GetInstance().CreateExplosionEffect(radius, transform.position, velocity);
            GameManager.GetInstance().CreateCollectibles(transform.position, radius, _leftDamageCurrency + _destroyDropCurrency);

            if (isBomb)
            {
                Shockwave s = Instantiate(shockwavePrefab, transform.position, Quaternion.identity).GetComponent<Shockwave>();
                s.Initialize(transform.position, .5f, explosionRadius/2);
                for (int i = 0; i < GameManager.GetInstance().meteors.Count; i++)
                {
                    if (GameManager.GetInstance().meteors[i] == this) { continue; }
                    if (Vector2.Distance(GameManager.GetInstance().meteors[i].transform.position, transform.position) < explosionRadius)
                    {
                        GameManager.GetInstance().meteors[i].DamageMeteor(bombDamage);
                    }
                }
            }
            Destroy(gameObject);
        }
    }
    public void ChangeVelocity(float freezetime, Vector2 newVelocity)
    {
        _freezeTimer = freezetime;
        velocity = newVelocity;
    }
    private void FixedUpdate()
    {
        if (_isDestroyed) { return; }

        _lastFrameTime = Time.time;
        _prevPos = transform.position;

        ApplyMovement();

        void ApplyMovement()
        {
            if (_freezeTimer > 0) { _freezeTimer -= Time.fixedDeltaTime; return; }

            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }
            
            if (( (transform.position.x + radius > GameManager.GetInstance().rectBounds.max.x && velocity.x > 0) ||
                (transform.position.x - radius < GameManager.GetInstance().rectBounds.min.x && velocity.x < 0) ) && _remainingBounces>0)
            {
                _remainingBounces--;
                velocity.x *= -1;
            }
            if (((transform.position.y + radius > GameManager.GetInstance().rectBounds.max.y && velocity.y > 0) ||
                (transform.position.y - radius < GameManager.GetInstance().rectBounds.min.y && velocity.y < 0)) && _remainingBounces > 0)
            {
                _remainingBounces--;
                velocity.y *= -1;
            }
            transform.position = transform.position + (Vector3)(velocity * Time.fixedDeltaTime);
            float velocityMagnitude = velocity.magnitude;
            kineticEnergy = mass * velocityMagnitude * velocityMagnitude * 0.5f;
        }
    }
}
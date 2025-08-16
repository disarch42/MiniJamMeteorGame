using UnityEngine;

public class Shockwave : MonoBehaviour
{
    private Material _ringMat;

    private float _ringAnimationTimer;
    private float _ringAnimationTime;
    private float _ringAnimationExpansion;
    public AnimationCurve ringAnimationCurve;

    public SpriteRenderer blackHole;
    public SpriteRenderer ring;
    
    private void Start()
    {
        _ringMat =  ring.material;
    }
    public void Initialize(Vector2 pos, float time, float blackHoleRadius)
    {
        _ringAnimationTimer = 0;
        _ringAnimationTime = time;
        _ringAnimationExpansion = blackHoleRadius*2;
        transform.position = pos;
    }
    private void Update()
    {
        _ringAnimationTimer += Time.deltaTime;

        float r = ringAnimationCurve.Evaluate(_ringAnimationTimer / _ringAnimationTime);
        r = Mathf.Max(0.01f, r);
        ring.transform.localScale = r * _ringAnimationExpansion * Vector3.one;
        _ringMat.SetFloat("_radius", r);

        blackHole.transform.localScale = r * Vector3.one;

        if (_ringAnimationTimer >= _ringAnimationTime)
        {
            Destroy(gameObject);
        }
    }
}

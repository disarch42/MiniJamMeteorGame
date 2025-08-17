using UnityEngine;

public class Shockwave : MonoBehaviour
{
    private Material _ringMat;

    private float _ringAnimationTimer;
    private float _ringAnimationTime;
    private float _ringAnimationExpansion;
    public AnimationCurve ringAnimationCurve;
    private BlackHoleRing _blackHoleRing;
    public SpriteRenderer blackHole;
    private void Start()
    {
        _blackHoleRing = GetComponent<BlackHoleRing>();
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
        _blackHoleRing.SetRadius(r * _ringAnimationExpansion / 2);

        blackHole.transform.localScale = r * Vector3.one;

        if (_ringAnimationTimer >= _ringAnimationTime)
        {
            Destroy(gameObject);
        }
    }
}

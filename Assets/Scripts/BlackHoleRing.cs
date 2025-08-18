using UnityEngine;

public class BlackHoleRing : MonoBehaviour
{
    private Material _ringMat;
    public SpriteRenderer ring;
    private void Awake()
    {
        _ringMat = ring.material;
    }
    public void SetRadius(float r)
    {
        ring.transform.localScale = r  * Vector3.one *2;
        _ringMat.SetFloat("_radius", r);
    }
}

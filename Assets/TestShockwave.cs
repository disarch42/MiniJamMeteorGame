using UnityEngine;

public class TestShockwave : MonoBehaviour
{
    float currentRadius;
    Material m;
    private void Start()
    {
        m = GetComponent<SpriteRenderer>().material;
    }
    private void Update()
    {
        currentRadius += Time.deltaTime;
        transform.localScale = Vector3.one * currentRadius;
        m.SetFloat("_radius", currentRadius);
    }
}

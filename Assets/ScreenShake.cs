using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;
    private Vector2 _shakeOffset;
    private struct ScreenShakeStruct
    {
        public float remainingTime;
        public float range;
        public ScreenShakeStruct(float shakeRange, float shakeDuration)
        {
            range = shakeRange;
            remainingTime = shakeDuration;
        }
    }
    private List<ScreenShakeStruct> _screenShakes = new List<ScreenShakeStruct>();

    private void Awake()
    {
        instance = this;
    }
    public void AddScreenShake(float range, float duration)
    {
        _screenShakes.Add(new ScreenShakeStruct(range, duration));
    }
    private void Update()
    {
        CalculateScreenShakeOffset();
        transform.position = new Vector3(_shakeOffset.x, _shakeOffset.y, -10.0f);
    }
    private void CalculateScreenShakeOffset()
    {
        float highestRange = 0;
        for (int i = _screenShakes.Count - 1; i >= 0; i--)
        {
            ScreenShakeStruct f = _screenShakes[i];
            f.remainingTime -= Time.unscaledDeltaTime;
            highestRange = Mathf.Max(highestRange, f.range);
            _screenShakes[i] = f;
            if (f.remainingTime <= 0)
            {
                _screenShakes.RemoveAt(i);
            }
        }
        _shakeOffset = Random.insideUnitCircle * highestRange;
    }
}

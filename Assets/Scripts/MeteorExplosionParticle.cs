using Unity.VisualScripting;
using UnityEngine;

public class MeteorExplosionParticle : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    public int particleAmount = 120;
    public float speed;
    public float initialSpeedMult;
    public float radius;
    public int originalWidth;
    public int originalHeight;
    public float originalPixelPerUnit;
    public int scaleDecrease;
    private int width;
    private int height;
    public float pixelPerUnit;
    public Vector2 size;
    private float _pixelSize;
    public bool emit;
    private void Update()
    {
        if (emit)
        {
            emit = false;
            Explode(radius, transform.position, Vector3.right*20);
        }
    }
    
    

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }
    

    public void Explode(float radius, Vector2 position, Vector2 initialSpeed)
    {
        transform.position = position;
        width = originalWidth / scaleDecrease;
        height = originalHeight / scaleDecrease;
        pixelPerUnit = originalPixelPerUnit / scaleDecrease;
        _pixelSize = 1.0f / pixelPerUnit;

        particleAmount = width * height;
        ParticleSystem.EmitParams a = new ParticleSystem.EmitParams();
        a.applyShapeToPosition = false;
        a.startSize = _pixelSize;
        _particleSystem.Emit(a, particleAmount);
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        int count = _particleSystem.GetParticles(particles);
        int idx = 0;
        for (int i = (count-particleAmount); i < count; i++)
        {
            idx++;
            int xp = idx % width;
            int yp = idx / height;
            float worldX = (xp - width * 0.5f) * _pixelSize;
            float worldY = (yp - height * 0.5f) * _pixelSize;
            particles[i].position = new Vector3(worldX, worldY, 0) + (Vector3)position;
            float u = Mathf.Clamp01((float)xp/(width-1.0f) );
            float v = Mathf.Clamp01((float)yp / (height - 1.0f));
            particles[i].velocity = (particles[i].position-transform.position).normalized*speed + (Vector3)(initialSpeed * initialSpeedMult);
            particles[i].startColor = new Color(u, v, 0, 1);
        }
        _particleSystem.SetParticles(particles, count);
    }
}

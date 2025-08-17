using Unity.VisualScripting;
using UnityEngine;

public class MeteorExplosionParticle : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    public int particleAmount = 120;
    public float speed;
    public float initialSpeedMult;
    public float radius;
    public int width;
    public int height;
    public int pixelPerUnit;
    
    
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

        ParticleSystem.EmitParams a = new ParticleSystem.EmitParams();
        a.applyShapeToPosition = false;
        _particleSystem.Emit(a, particleAmount);
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        int count = _particleSystem.GetParticles(particles);
        for (int i = (count-particleAmount); i < count; i++)
        {
            //Vector2 r = Random.insideUnitCircle;
            //Vector2 localPos = r * radius; 

            int xp = i % width;
            int yp = i / height;

            Vector2 localPos = transform.position;
            particles[i].position = transform.position + (Vector3)localPos;
            float u = Mathf.Clamp01( (pos.x + 1.0f) /2.0f);
            float v = Mathf.Clamp01( (pos.y + 1.0f) / 2.0f);
            particles[i].velocity = new Vector3(r.x, r.y, 0)*speed + (Vector3)(initialSpeed * initialSpeedMult);
            particles[i].startColor = new Color(u, v, 0, 1);
        }
        //velocityOverLifetimeModule.speedModifierMultiplier
        _particleSystem.SetParticles(particles, count);
    }
}

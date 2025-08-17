using Unity.VisualScripting;
using UnityEngine;

public class MeteorExplosionParticle : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    public int particleAmount = 120;
    public float speed;
    
    /*
    public bool emit;
    private void Update()
    {
        if (emit)
        {
            emit = false;
            Explode(2, transform.position, Vector3.right*20);
        }
    }
    */
    

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
        /*
        for (int i = count - 1; i >= particleAmount; i--)
        {
        */
        for (int i = (count-particleAmount); i < count; i++)
        {
            Vector2 r = Random.insideUnitCircle;
            Vector2 localPos = r * radius; 
            particles[i].position = transform.position + (Vector3)localPos;
            float u = Mathf.Clamp01( (r.x + 1.0f) /2.0f);
            float v = Mathf.Clamp01( (r.y + 1.0f) / 2.0f);
            particles[i].velocity = new Vector3(r.x, r.y, 0);
            particles[i].startColor = new Color(u, v, 0, 1);
        }
        //velocityOverLifetimeModule.speedModifierMultiplier
        _particleSystem.SetParticles(particles, count);
    }
}

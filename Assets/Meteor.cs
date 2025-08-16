using UnityEngine;

public class Meteor : MonoBehaviour
{
    public Vector2 velocity;
    public float radius;
    public float mass;
    public float kineticEnergy;
    // since meteors will probably be cached having a function that resets all values will be nice
    void InitializeMeteor(float _radius, float _mass, Vector2 initialVelocity)
    {
        if (_radius == 0) { _radius = transform.localScale.x; }
        if (_mass == 0) { _mass = 1; }
        radius = _radius;
        mass = _mass;
        velocity = initialVelocity;
        
    }
    private void Start()
    {
        InitializeMeteor(radius, mass, velocity);
        GameManager.GetInstance().meteors.Add(this);
    }
    private void OnDestroy()
    {
        GameManager.GetInstance().meteors.Remove(this);
    }
    private void FixedUpdate()
    {
        if ( ( transform.position.x + radius > GameManager.GetInstance().rectBounds.max.x  && velocity.x>0) || 
            (transform.position.x - radius < GameManager.GetInstance().rectBounds.min.x&&velocity.x<0))
        {
            velocity.x *= -1;
        }
        if ((transform.position.y + radius > GameManager.GetInstance().rectBounds.max.y && velocity.y > 0) ||
            (transform.position.y - radius < GameManager.GetInstance().rectBounds.min.y && velocity.y < 0))
        {
            velocity.y *= -1;
        }

        transform.position = transform.position + (Vector3)(velocity * Time.fixedDeltaTime);

        float velocityMagnitude = velocity.magnitude;
        kineticEnergy = mass * velocityMagnitude*velocityMagnitude * 0.5f;
    }
}

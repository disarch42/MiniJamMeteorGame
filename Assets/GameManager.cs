using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public Bounds rectBounds;
    public List<Meteor> meteors;
    [Range(0.0f,2.0f)]
    public float collisionVelocityMult;
    static private GameManager _current;
    static public GameManager GetInstance() { return _current; }
    private void Awake()
    {
        _current = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(rectBounds.center, rectBounds.size);
    }

    private void FixedUpdate()
    {
        CheckMeteorCollisions();

        void CheckMeteorCollisions()
        {
            for (int i = 0; i < meteors.Count; i++)
            {
                for (int j = i + 1; j < meteors.Count; j++)
                {
                    float dist = Vector2.Distance(meteors[i].transform.position, meteors[j].transform.position);
                    if (dist < meteors[i].radius + meteors[j].radius)
                    {
                        Meteor meteorA = meteors[i];
                        Meteor meteorB = meteors[j];

                        Vector2 collisionNormal = ((Vector2)meteorB.transform.position - (Vector2)meteorA.transform.position).normalized;
                        Vector2 collisionTangent = new Vector2(-collisionNormal.y, collisionNormal.x);

                        float meteorATangent = Vector2.Dot(meteorA.velocity, collisionTangent);
                        float meteorBTangent = Vector2.Dot(meteorB.velocity, collisionTangent);

                        //these swap
                        float meteorANormal = Vector2.Dot(meteorA.velocity, collisionNormal);
                        float meteorBNormal = Vector2.Dot(meteorB.velocity, collisionNormal);

                        float meteorANormalPrev = meteorANormal;

                        meteorANormal = (meteorANormal * (meteorA.mass - meteorB.mass) + 2 * meteorB.mass * meteorBNormal) / (meteorA.mass + meteorB.mass);
                        meteorBNormal = (meteorBNormal * (meteorB.mass - meteorA.mass) + 2 * meteorA.mass * meteorANormalPrev) / (meteorA.mass + meteorB.mass);

                        meteorA.velocity = meteorANormal * collisionNormal + meteorATangent * collisionTangent;
                        meteorB.velocity = meteorBNormal * collisionNormal + meteorBTangent * collisionTangent;

                        meteorA.velocity *= collisionVelocityMult;
                        meteorB.velocity *= collisionVelocityMult;

                        float p = meteorA.radius + meteorB.radius - dist;
                        Vector2 meteorApos = meteorA.transform.position;
                        meteorApos -= (p * (1.0f / meteorA.mass) * collisionNormal) / ((1.0f / meteorA.mass) + (1.0f / meteorB.mass));

                        Vector2 meteorBpos = meteorB.transform.position;
                        meteorBpos += (p * (1.0f / meteorB.mass) * collisionNormal) / ((1.0f / meteorA.mass) + (1.0f / meteorB.mass));

                        meteorA.transform.position = new Vector3(meteorApos.x, meteorApos.y, meteorA.transform.position.z);
                        meteorB.transform.position = new Vector3(meteorBpos.x, meteorBpos.y, meteorB.transform.position.z);
                    }
                }
            }
        }
    }
}

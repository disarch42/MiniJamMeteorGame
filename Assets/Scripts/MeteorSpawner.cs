using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{

    public static MeteorSpawner instance;

    public Meteor meteorPrefab;

    /*
    private void Start()
    {
        StartCoroutine(SpawnMeteorLoop());
    }

    public IEnumerator SpawnMeteorLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            SpawnMeteor();
        }
    }
    */

    private void FixedUpdate()
    {
        if(Random.Range(0.0f, 1.0f) <= StatsManager.instance.meteorSpawnChance)
        {
            SpawnMeteor();
        }
    }

    public void SpawnMeteor()
    {
        float r = Random.Range(.5f, StatsManager.instance.meteorRadius);
        Vector3 spawnPos = GetMeteorSpawnPos(r);
        Meteor meteorInstance = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
        meteorInstance.InitializeMeteor(r, 5*r, 1.0f, 10, 0.2f, GetMeteorVelocity(5));
    }
    //meteor velocity direction doesnt need to point in as the levels have bounds
    public Vector2 GetMeteorVelocity(float mag)
    {
        return Random.insideUnitCircle * mag;
    }
    public Vector3 GetMeteorSpawnPos(float radius)
    {
        Vector3 spawnPos = Vector3.zero;
        //enter from top/bottom
        if(Random.Range(0,2)==0)
        {
            spawnPos.y = (Random.Range(0, 2) == 0) ? (GameManager.GetInstance().rectBounds.max.y+radius) : (GameManager.GetInstance().rectBounds.min.y-radius);
            spawnPos.x = Random.Range(GameManager.GetInstance().rectBounds.min.x - radius, GameManager.GetInstance().rectBounds.max.x + radius);
        }
        //enter from left/right
        else
        {
            spawnPos.x = (Random.Range(0, 2) == 0) ? (GameManager.GetInstance().rectBounds.max.x + radius) : (GameManager.GetInstance().rectBounds.min.x - radius);
            spawnPos.y = Random.Range(GameManager.GetInstance().rectBounds.min.y - radius, GameManager.GetInstance().rectBounds.max.y + radius);
        }
        return spawnPos;
    }
}

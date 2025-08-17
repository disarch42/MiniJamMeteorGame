using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{

    public static MeteorSpawner instance;

    public List<Meteor> meteorPrefabs;
    private float _spawnTimer;
    private bool _spawned = false;
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
        if(!_spawned && Random.Range(0.0f, 1.0f) <= StatsManager.instance.meteorSpawnChance)
        {
            SpawnMeteor();
            _spawned = true;
            _spawnTimer = 0.25f;
        }
        else if (_spawned && _spawnTimer<=0)
        {
            SpawnMeteor();
            _spawned = false;
        }
        _spawnTimer -= Time.fixedDeltaTime;

    }

    public void SpawnMeteor()
    {
        float r = Random.Range(.5f, StatsManager.instance.meteorRadius);
        bool fromTop = Random.Range(0, 2) == 0;
        bool fromRight = Random.Range(0, 2) == 0;
        Vector3 spawnPos = GetMeteorSpawnPos(r, fromRight, fromTop);
        Meteor meteorInstance = Instantiate(meteorPrefabs[Random.Range(0,meteorPrefabs.Count)], spawnPos, Quaternion.identity);
        meteorInstance.InitializeMeteor(r, 3, 5*r, 1.0f, 10, 0.2f, GetMeteorVelocity(5, fromRight, fromTop));
    }
    //meteor velocity direction doesnt need to point in as the levels have bounds
    public Vector2 GetMeteorVelocity(float mag, bool fromRight, bool fromTop)
    {
        float xMult = fromRight ? -1 :1;
        float yMult = fromTop ? -1 : 1;
        return new Vector2(Random.Range(0.0f,1.0f)*xMult, Random.Range(0.0f, 1.0f)*yMult).normalized * mag;
    }
    public Vector3 GetMeteorSpawnPos(float radius, bool fromRight, bool fromTop)
    {
        Vector3 spawnPos = Vector3.zero;
        //enter from top/bottom
        if(Random.Range(0,2)==0)
        {
            spawnPos.y = fromTop ? (GameManager.GetInstance().rectBounds.max.y+radius) : (GameManager.GetInstance().rectBounds.min.y-radius);
            spawnPos.x = fromRight ? Random.Range(0, GameManager.GetInstance().rectBounds.max.x + radius) : Random.Range(GameManager.GetInstance().rectBounds.min.x - radius, 0);
        }
        //enter from left/right
        else
        {
            spawnPos.x = fromRight ? (GameManager.GetInstance().rectBounds.max.x + radius) : (GameManager.GetInstance().rectBounds.min.x - radius);
            spawnPos.y = fromTop ? Random.Range(0.0f, GameManager.GetInstance().rectBounds.max.y + radius) : Random.Range(GameManager.GetInstance().rectBounds.min.y - radius, 0.0f);
        }
        return spawnPos;
    }
}

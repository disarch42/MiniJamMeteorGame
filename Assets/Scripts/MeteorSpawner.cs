using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{

    public static MeteorSpawner instance;

    public Meteor meteorPrefab;
  

    private void Start()
    {
        StartCoroutine(SpawnMeteorLoop());
    }

    public IEnumerator SpawnMeteorLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            SpawnMeteor();
        }
    }
    public void SpawnMeteor()
    {

        Vector3 spawnPos = GetMeteorSpawnPos();
        Vector3 spawnRot = GetMeteorSpawnRot(spawnPos);
        Meteor meteorInstance = Instantiate(meteorPrefab, spawnRot, Quaternion.Euler(spawnRot));
        meteorInstance.radius = Random.Range(1,Stats.meteorRadius);
    }

    public Vector3 GetMeteorSpawnRot(Vector3 pos)
    {
        return Vector3.zero;
    }
    public Vector3 GetMeteorSpawnPos()
    {
        return Vector3.zero;
    }
}

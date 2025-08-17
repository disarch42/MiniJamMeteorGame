using UnityEngine;

using System.Collections.Generic;

public class StatsManager:MonoBehaviour
{
    public static StatsManager instance;

     List<UpgradeObject> upgradeObjects = new List<UpgradeObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public  float meteorHealthMult=1;
    public  float meteorSpeedMult=1;
    public  float meteorRadius=.5f;



    public  float minBlackHoleRadius = 0.5f;
    public  float minBlackholeChargeTime = 0.06f;
    public  float maxBlackholeChargeTime = 0.13f;
    public  float blackHoleFreezeTime = 0.25f;

    public float startingHealth;
    public  float mouseCollectRadius = 0.25f;

    public  float chargingTimeScale = 0.5f;


    [Header("meteor spawner always takes a random value between min meteor size and this, and then assigns the ")]
    public float MaxMeteorSize = 1;

    public float BlackHoleCost = 15;
    public float maxBlackHoleRadius = 1.5f;
    public float bombMeteorChance = 0;
    public float magnetRadius = 0.2f;

    public float meteorDamageOnHit = 10f;
    public float meteorSpawnChance = 0.01f;
    public float moneyGainRate = 1;
    public float pickupHealRate = 0;
    public float decayIncreaseRate = 0.01f;
}

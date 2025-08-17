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
    public  float meteorSpawnChance=0.01f;
    public  float meteorSpeedMult=1;
    public  float meteorRadius=.5f;


    public  float maxBlackHoleRadius=1.5f;

    public  float minBlackHoleRadius = 0.5f;
    public  float minBlackholeChargeTime = 0.06f;
    public  float maxBlackholeChargeTime = 0.13f;
    public  float blackHoleFreezeTime = 0.25f;


    public  float mouseCollectRadius = 0.25f;

    public  float chargingTimeScale = 0.5f;
   


}

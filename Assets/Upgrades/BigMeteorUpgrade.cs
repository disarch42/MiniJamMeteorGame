using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
[CreateAssetMenu(fileName = "MeteorHPUpgrade", menuName = "Upgrades/BigMeteorUpgrade")]
public class BigMeteorUpgrade : Upgrade
{
    public List<float> upgradeValues = new List<float>();

    public override void OnUpgrade(int step)
    {
        base.OnUpgrade(step);
        // Additional logic for MeteorHPUpgrade
        Debug.Log("Meteor HP Upgrade applied!" + step);
        StatsManager.instance.MaxMeteorSize += upgradeValues[step]; 
        // Increase the health of meteors or perform other upgrade-related actions
    }
}
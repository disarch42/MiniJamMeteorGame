using UnityEngine;

using System.Collections.Generic;
[System.Serializable]
[CreateAssetMenu(fileName = "MeteorHPUpgrade", menuName = "Upgrades/BombMeteorChanceUpgrade")]
public class BombMeteorChanceUpgrade : Upgrade
{
    [Header("Chance to spawn a bomb meteor on every second 1 is %100")]
    public List<float> upgradeValues = new List<float>();

    public override void OnUpgrade(int step)
    {
        base.OnUpgrade(step);
        // Additional logic for MeteorHPUpgrade
        StatsManager.instance.bombMeteorChance += upgradeValues[step];
        Debug.Log("Meteor HP Upgrade applied!" + step);
        // Increase the health of meteors or perform other upgrade-related actions
    }
}
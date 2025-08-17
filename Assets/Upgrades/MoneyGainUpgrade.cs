using UnityEngine;

using System.Collections.Generic;
[System.Serializable]
[CreateAssetMenu(fileName = "MeteorHPUpgrade", menuName = "Upgrades/MoneyGainUpgrade")]
public class MoneyGainUpgrade : Upgrade
{
    [Header("perc")]
    public List<float> upgradeValues = new List<float>();

    public override void OnUpgrade(int step)
    {
        base.OnUpgrade(step);
        StatsManager.instance.moneyGainRate += upgradeValues[step];
        // Additional logic for MeteorHPUpgrade
        Debug.Log("Meteor HP Upgrade applied!" + step);
        // Increase the health of meteors or perform other upgrade-related actions
    }
}
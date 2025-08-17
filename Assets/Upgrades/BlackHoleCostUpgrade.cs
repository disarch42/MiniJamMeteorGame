using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
[CreateAssetMenu(fileName = "MeteorHPUpgrade", menuName = "Upgrades/BlackHoleCostUpgrade")]
public class BlackHoleCostUpgrade : Upgrade
{
    [Header("percantage drop")]
    public List<float> upgradeValues = new List<float>();

    public override void OnUpgrade(int step)
    {
        base.OnUpgrade(step);
        // Additional logic for MeteorHPUpgrade

        StatsManager.instance.BlackHoleCost *=1-upgradeValues[step];
        Debug.Log("Meteor HP Upgrade applied!" + step);
        // Increase the health of meteors or perform other upgrade-related actions
    }
}

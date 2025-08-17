using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MeteorHPUpgrade", menuName = "Upgrades/MeteorDamageUpgrade")]
public class MeteorDamageUpgrade : Upgrade
{
    public List<float> upgradeValues = new List<float>();

    public override void OnUpgrade(int step)
    {
        base.OnUpgrade(step);

        StatsManager.instance.meteorDamageOnHit += upgradeValues[step];

        // Additional logic for MeteorHPUpgrade
        Debug.Log("Meteor HP Upgrade applied!" + step);
        // Increase the health of meteors or perform other upgrade-related actions
    }
}

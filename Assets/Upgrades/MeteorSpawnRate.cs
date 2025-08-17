using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MeteorSpeedUpgrades", menuName = "Upgrades/MeteorSpawnRate")]
public class MeteorSpawnRate : Upgrade
{
    public List<float> upgradeValues = new List<float>();
    public override void OnUpgrade(int step)
    {
        base.OnUpgrade(step);
        // Additional logic for MeteorSpeedUpgrade
        Debug.Log("Meteor Speed Upgrade applied!" + step);
        // Increase the speed of meteors or perform other upgrade-related actions
    }
}

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "MeteorSpeedUpgrade", menuName = "Upgrades/MeteorSpeedUpgrade")]
public class MeteorSpeedUpdate : Upgrade
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
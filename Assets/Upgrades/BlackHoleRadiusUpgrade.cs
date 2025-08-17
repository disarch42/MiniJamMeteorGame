using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "BlackHoleRadiusUpgrade", menuName = "Upgrades/BlackHoleRadiusUpgrade")]
public class BlackHoleRadiusUpgrade : Upgrade
{
    public List<float> upgradeValues = new List<float>();
    public override void OnUpgrade(int step)
    {
        base.OnUpgrade(step);
        // Additional logic for BlackHoleRadiusUpgrade
        Debug.Log("Black Hole Radius Upgrade applied!" + step);
        StatsManager.instance.maxBlackHoleRadius += upgradeValues[step];
        // Increase the radius of black holes or perform other upgrade-related actions
    }
}
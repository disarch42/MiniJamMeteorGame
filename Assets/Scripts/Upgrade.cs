using UnityEngine;

using System.Collections.Generic;
public class Upgrade:ScriptableObject
{
    public Sprite icon;
    public List<int> upgradeCosts = new List<int>();
    public int costType;
    public string upgradeDescription;
    public virtual void OnUpgrade(int step)
    {
        // This method will be called when the upgrade is applied
        Debug.Log("Upgrade applied!");
    }



}

[System.Serializable]
[CreateAssetMenu(fileName = "MeteorHPUpgrade", menuName = "Upgrades/MeteorHPUpgrade")]
public class MeteorHPUpgrade : Upgrade
{
    public List<float> upgradeValues = new List<float>();

    public override void OnUpgrade(int step)
    {
        base.OnUpgrade(step);
        // Additional logic for MeteorHPUpgrade
        Debug.Log("Meteor HP Upgrade applied!"+step);
        // Increase the health of meteors or perform other upgrade-related actions
    }
}
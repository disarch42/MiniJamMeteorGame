using UnityEngine;

using System.Collections.Generic;
[System.Serializable]
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









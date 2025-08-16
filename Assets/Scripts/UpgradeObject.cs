using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeObject : MonoBehaviour, IPointerClickHandler
{


    public Upgrade assignedUpgrade;
    public int upgradeStep;
    private void Start()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        GetComponent<Image>().sprite = assignedUpgrade.icon;
    }


    public void OnClick()
    {
        //check cost, if enough, apply upgrade
        bool canUpgrade = upgradeStep < assignedUpgrade.upgradeCosts.Count;
        if (!canUpgrade)
        {
            Debug.Log("Upgrade not available or already maxed out.");
            return;
        }
        bool affordable = CurrencyManager.Instance.CheckAffordable(assignedUpgrade.costType, GetUpgradeCost());
        if (!affordable)
        {

            Debug.Log("Not enough currency to upgrade.");
            return;
        }
        CurrencyManager.Instance.SpendCurrency(assignedUpgrade.costType, GetUpgradeCost());
        assignedUpgrade.OnUpgrade(upgradeStep);
        upgradeStep++;
    }

    public int GetUpgradeCost()
    {
        return assignedUpgrade.upgradeCosts[upgradeStep];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }
}

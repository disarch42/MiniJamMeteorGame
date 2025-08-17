using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeObject : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler, IPointerExitHandler
{

    public Image border;
    public Image icon;
    public TextMeshProUGUI upgradeStepText;
    public TextMeshProUGUI cost;
    public GameObject description;
    public TextMeshProUGUI upgradeDescription;
    public Upgrade assignedUpgrade;
    public int upgradeStep;

    bool mouseOver = false;
    float mouseOverCounter;
    private void Start()
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        icon.sprite = assignedUpgrade.icon;
        upgradeStepText.text =  + (upgradeStep) + "/" + assignedUpgrade.upgradeCosts.Count;
        upgradeDescription.text = assignedUpgrade.upgradeDescription;

       

        if (upgradeStep >= assignedUpgrade.upgradeCosts.Count)
        {
            cost.text = "";
            border.color = Color.black;

        }
        else
        {
            cost.text = GetUpgradeCost().ToString() + " ";
            if (CurrencyManager.Instance.CheckAffordable(assignedUpgrade.costType, GetUpgradeCost()))
            {
                border.color = Color.green;
            }
            else
            {
                border.color = Color.red;
            }
        }


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
        UpdateVisuals();
    }

    public int GetUpgradeCost()
    {
        return assignedUpgrade.upgradeCosts[upgradeStep];
    }

    private void Update()
    {
        if (mouseOver) HoldMouse();
    }

    public void HoldMouse()
    {
        mouseOverCounter += Time.deltaTime;
        if (mouseOverCounter > 0.5f)
        {
            description.SetActive(true);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver=true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        mouseOverCounter = 0f;
        description.SetActive(false);
    }
}

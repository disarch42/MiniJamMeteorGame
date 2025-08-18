using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
public class UpgradeObject : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI upgradeStepText;
    public TextMeshProUGUI cost;
    public RectTransform description;
    public TextMeshProUGUI upgradeDescription;
    public Upgrade assignedUpgrade;
    public int upgradeStep;
    public Vector2 startLocalPos;
    public Vector2 endLocalPosUpgFinished;
    public Vector2 startLocalScale;
    public Vector2 endLocalPos;
    public Vector2 endLocalScale;
    public float animTime = 0.5f;
    public Button button;
    bool mouseOver = false;
    float mouseOverCounter;
    private Color _priceTextCol;
    private Color _stepTextCol;
    public List<UpgradeObject> requiredPrevUpgrades;
    public static UnityEvent refreshUpgrades = new UnityEvent();
    bool cantUpgrade;

    private void Start()
    {
        _priceTextCol = cost.color;
        _stepTextCol = upgradeStepText.color;
        UpdateVisuals();
        description.gameObject.SetActive(true);
        refreshUpgrades.AddListener(UpdateVisuals);
    }

    public void UpdateVisuals()
    {
        cantUpgrade = CheckCantUpgrade();
        if (cantUpgrade)
        {
            icon.sprite = assignedUpgrade.icon;
            icon.color = new Vector4(.5f, .5f, .5f, 1.0f);
            cost.color = new Color(_priceTextCol.r * .5f, _priceTextCol.g * .5f, _priceTextCol.b * .5f, 1.0f);
            upgradeStepText.color = new Color(_stepTextCol.r * .5f, _stepTextCol.g * .5f, _stepTextCol.b * .5f, 1.0f);
            button.enabled = false;

            cost.text = "";
            upgradeStepText.text = "";
            upgradeDescription.text = "Unlock previous upgrades first!";
            return;
        }
        upgradeStepText.text = +(upgradeStep) + "/" + assignedUpgrade.upgradeCosts.Count;
        upgradeDescription.text = assignedUpgrade.upgradeDescription;
        button.enabled = true;
        cost.color = _priceTextCol;
        upgradeStepText.color = _stepTextCol;


        if (upgradeStep >= assignedUpgrade.upgradeCosts.Count)
        {
            mouseOver = false;
            cost.text = "";
            icon.sprite = assignedUpgrade.fullIcon;
            icon.color = Color.white;
            button.enabled = false;
        }
        else
        {
            cost.text = GetUpgradeCost().ToString() + " ";
            if (CurrencyManager.Instance.CheckAffordable(assignedUpgrade.costType, GetUpgradeCost()))
            {
                icon.sprite = assignedUpgrade.icon;
                icon.color = Color.white;
            }
            else
            {
                button.enabled = false;
                icon.sprite = assignedUpgrade.icon;
                icon.color = new Vector4(.5f, .5f, .5f, 1.0f);
                cost.color = new Color(_priceTextCol.r * .5f, _priceTextCol.g * .5f, _priceTextCol.b * .5f, 1.0f);
                upgradeStepText.color = new Color(_stepTextCol.r * .5f, _stepTextCol.g * .5f, _stepTextCol.b * .5f, 1.0f);
            }
        }
    }

    bool CheckCantUpgrade()
    {
        cantUpgrade = false;
        for (int i = 0; i < requiredPrevUpgrades.Count; i++)
        {
            if (requiredPrevUpgrades[i].upgradeStep <= 0)
            {
                cantUpgrade = true;
            }
        }
        return cantUpgrade;
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
        SoundManager.current.PlayOneShot(SoundManager.current.buyClip);
        CurrencyManager.Instance.SpendCurrency(assignedUpgrade.costType, GetUpgradeCost());
        assignedUpgrade.OnUpgrade(upgradeStep);
        upgradeStep++;
        refreshUpgrades.Invoke();
        //UpdateVisuals();
    }

    public int GetUpgradeCost()
    {
        return assignedUpgrade.upgradeCosts[upgradeStep];
    }

    private void Update()
    {
        if (mouseOver)
        {
            mouseOverCounter = Mathf.Max(0, mouseOverCounter);
            mouseOverCounter += Time.deltaTime;
        }
        else
        {
            mouseOverCounter = Mathf.Min(animTime, mouseOverCounter);
            mouseOverCounter -= Time.deltaTime;
        }
        float t = mouseOverCounter / animTime;
        description.localPosition = Vector3.Slerp(startLocalPos, (upgradeStep >= assignedUpgrade.upgradeCosts.Count || cantUpgrade) ? endLocalPosUpgFinished : endLocalPos, t);
        description.localScale = Vector3.Slerp(startLocalScale, endLocalScale, t);
    }
    public void OnPointerEnter()
    {
        SoundManager.current.PlayOneShot(SoundManager.current.hoverClip);
        mouseOver=true;
    }

    public void OnPointerExit()
    {
        mouseOver = false;
    }
}

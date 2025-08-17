using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CurrencyManager : MonoBehaviour
{

    public static CurrencyManager Instance;

  public List<int> CurrencyList = new List<int>();
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }


    public void GainCurrency(int type,int amount)
    {
        CurrencyList[type] += amount;
        PlayerHud.instance.GainCurrency(type, amount);
    }


    public void SpendCurrency(int type, int amount)
    {

        CurrencyList[type] -= amount;

        PlayerHud.instance.LooseCurrency(type, amount);
    }

    public bool CheckAffordable(int type,int amount)
    {
        if (CurrencyList[type] >= amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

using TMPro;
using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    public static PlayerHud instance;

    public TextMeshProUGUI playerCurrency;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        playerCurrency.text = CurrencyManager.Instance.CurrencyList[0].ToString();
    }

    public void GainCurrency(int type, int amount)
    {
      
        playerCurrency.text = CurrencyManager.Instance.CurrencyList[type].ToString();
    }

    public void LooseCurrency(int type, int amount)
    {
        playerCurrency.text = CurrencyManager.Instance.CurrencyList[type].ToString();
    }
}

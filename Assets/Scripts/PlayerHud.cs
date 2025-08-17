using System.Collections;
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
        StartCoroutine(PunchScale(playerCurrency.transform));
    }

    public void LooseCurrency(int type, int amount)
    {
        playerCurrency.text = CurrencyManager.Instance.CurrencyList[type].ToString();
        StartCoroutine(PunchScale(playerCurrency.transform));
    }

    private IEnumerator PunchScale(Transform target)
    {
        Vector3 original = Vector3.one;
        Vector3 enlarged = original * 1.2f;

        float duration = 0.15f;
        float time = 0;

        // Scale up
        while (time < duration)
        {
            time += Time.deltaTime;
            target.localScale = Vector3.Lerp(original, enlarged, time / duration);
            yield return null;
        }

        // Scale back
        time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            target.localScale = Vector3.Lerp(enlarged, original, time / duration);
            yield return null;
        }

        target.localScale = original; // ✅ force reset
    }
}

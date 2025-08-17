using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private const float _returnTime=0.1f;
    private float _money;
    
    public void InitializeCollectible(float money) 
    { 
        StopAllCoroutines();
        _money = money;
    }

    //removal from availables is managed in gamemanager, adding to cache is managed from this script
    public void OnCollect(Vector2 point, float wait)
    {
        StartCoroutine(collectAnim(_returnTime, transform.position, point,wait));
    }
    private IEnumerator collectAnim(float duration, Vector2 startPoint, Vector2 endPoint, float wait)
    {
        yield return new WaitForSeconds(wait);
        float elapsed=0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector2.Lerp(startPoint, endPoint, elapsed / duration);

            yield return new WaitForEndOfFrame();
        }
        CurrencyManager.Instance.GainCurrency(0, (int)(_money*StatsManager.instance.moneyGainRate));
        HealthbarController.instance.HealthChange(StatsManager.instance.pickupHealRate);
        gameObject.SetActive(false);
        GameManager.GetInstance().cachedCollectibles.Add(this);
        
    }
}

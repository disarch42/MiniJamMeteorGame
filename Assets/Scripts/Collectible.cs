using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private const float _returnTime=0.1f;
    private float _money;
    public List<Sprite> sprites = new List<Sprite>();
    public SpriteRenderer spriteRend;
    private float _randomOffset;
    const float _hoverSpeed = 0.1f;
    private void Start()
    {
        spriteRend.sprite = sprites[Random.Range(0, sprites.Count)];
        _randomOffset = Random.Range(0, 5.0f);
    }
    public void InitializeCollectible(float money) 
    {
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0.0f, 360.0f));
        StopAllCoroutines();
        _money = money;
    }

    //removal from availables is managed in gamemanager, adding to cache is managed from this script
    public void OnCollect(Vector2 point, float wait)
    {
        StartCoroutine(collectAnim(_returnTime, transform.position, point,wait));
    }
    private void Update()
    {
        spriteRend.transform.position = transform.position + new Vector3(0, Mathf.PingPong(Time.time*_hoverSpeed+_randomOffset, 0.2f));
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

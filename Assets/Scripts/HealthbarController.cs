using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HealthbarController : MonoBehaviour
{
    public static HealthbarController instance;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public Image healthBarImage; // Reference to the UI Image component for the health bar
    public float health = 100f;
    public float healthDrainRate = 5;
    private void Start()
    {
        health = StatsManager.instance.startingHealth;
        UpdateHealthBar();
    }


    private void FixedUpdate()
    {
        HealthChange(-Time.fixedDeltaTime * healthDrainRate);
        healthDrainRate += 0.01f;

    }

    public void HealthChange(float amount)
    {
        health += amount;
        UpdateHealthBar();

        if(health <= 0)
        {
            Death();
        }
    }

    public void UpdateHealthBar()
    {
        healthBarImage.fillAmount = health / StatsManager.instance.startingHealth;
    }

    public void Death()
    {
        SceneManager.LoadScene("MainScene");
    }
}

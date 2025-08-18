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

    public Slider slider;
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
        healthDrainRate += StatsManager.instance.decayIncreaseRate;

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
        slider.value = (health / StatsManager.instance.startingHealth);
    }
    private bool alreadyDead = false;
    public void Death()
    {
        if (!alreadyDead)
        {
            alreadyDead = true;
            MainMenuManager.instance.EndRound();
        }
    }
}

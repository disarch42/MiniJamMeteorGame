using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        MainCanvas.instance.Show();
    }
    public void StartRound()
    {
        // Load the game scene
        MainCanvas.instance.Hide();
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}

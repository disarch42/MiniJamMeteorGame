using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

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

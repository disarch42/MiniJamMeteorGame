using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
 public void StartRound()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}

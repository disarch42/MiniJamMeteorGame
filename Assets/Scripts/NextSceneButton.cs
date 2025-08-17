using UnityEngine;

public class NextSceneButton : MonoBehaviour
{
   public void NextScene()
    {
        MainMenuManager.instance.StartRound();
    }
}

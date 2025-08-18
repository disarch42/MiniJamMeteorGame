using System.Collections;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    private bool destroyed = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            MainCanvas.instance.Show();
        }
        else
        {
            destroyed = true;
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        if (!destroyed) { MainCanvas.instance.Show(); }
    }
    public void StartRound()
    {
        // Load the game scene
        StopAllCoroutines();
        MainCanvas.instance.Hide();
        StartCoroutine(f(.2f, true, "SampleScene"));
    }
    public void EndRound()
    {
        StopAllCoroutines();
        MainCanvas.instance.Show();
        StartCoroutine(f(.2f, true, "MainScene"));
        UpgradeObject.refreshUpgrades.Invoke();
    }
    IEnumerator f(float t, bool r, string scene)
    {
        float elapsed = 0;
        while (elapsed < t)
        {
            elapsed += Time.deltaTime;
            MainCanvas.instance.transition.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, r ? (elapsed / t) : (1 - (elapsed / t))));
            yield return new WaitForEndOfFrame();
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        StartCoroutine(c(.2f,!r));
    }
    IEnumerator c(float t, bool r)
    {
        float elapsed = 0;
        while (elapsed < t)
        {
            yield return new WaitForEndOfFrame();
            elapsed += Time.deltaTime;
            MainCanvas.instance.transition.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, r ? (elapsed / t) : (1 - (elapsed / t))));
        }
        //UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}

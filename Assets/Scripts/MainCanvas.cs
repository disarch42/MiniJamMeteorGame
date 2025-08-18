using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    public GameObject contents;
    public Image transition;
    public static MainCanvas instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Show();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Add any additional methods or properties needed for the MainCanvas here

    public void Hide()
    {
        contents.SetActive(false);
    }

    public void Show()
    {
        contents.SetActive(true);
    }
}

using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public GameObject contents;
    public static MainCanvas instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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

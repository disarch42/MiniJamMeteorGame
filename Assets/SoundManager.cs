using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager current;
    public AudioSource music;
    public AudioSource sfx;
    
    public AudioClip hoverClip;
    public AudioClip buyClip;
    public AudioClip crashClip;
    public AudioClip explosionClip;
    private void Awake()
    {
        if (current == null)
        {
            current = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayOneShot(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }

}

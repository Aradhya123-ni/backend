using UnityEngine;


// create new class for singleton base function and inherit other classes which need to be singleton
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;
    public AudioClip clickSound;

    private bool soundOn = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClick()
    {
        if (soundOn)
            audioSource.PlayOneShot(clickSound);
    }

    public void ToggleSound()
    {
        soundOn = !soundOn;
    }

    public bool IsSoundOn()
    {
        return soundOn;
    }
}



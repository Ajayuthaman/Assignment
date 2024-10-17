using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip backgroundMusic;
    public AudioClip wrongSound;
    public AudioClip correctSound;
    public AudioClip winSound;
    public AudioClip flipSound;

    public AudioSource musicSource;
    public AudioSource soundEffectSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

/*            // Initialize audio sources
            musicSource = gameObject.AddComponent<AudioSource>();
            soundEffectSource = gameObject.AddComponent<AudioSource>();*/
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBackgroundMusic()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayWrongSound()
    {
        soundEffectSource.PlayOneShot(wrongSound);
    }

    public void PlayCorrectSound()
    {
        soundEffectSource.PlayOneShot(correctSound);
    }

    public void PlayWinSound()
    {
        soundEffectSource.PlayOneShot(winSound);
    }

    public void PlayFlipSound()
    {
        soundEffectSource.PlayOneShot(flipSound);
    }

    public void StopAllSound()
    {
        soundEffectSource.Stop();
        musicSource.Stop();
    }
}

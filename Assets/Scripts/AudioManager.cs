using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip backgroundMusic;
    public AudioClip wrongSound;
    public AudioClip correctSound;
    public AudioClip winSound;
    public AudioClip flipSound;
    public AudioClip failSound;

    public AudioSource musicSource;
    public AudioSource soundEffectSource;

    //mute sprites
    public Image muteImg;
    public Sprite[] muteIcons;

    private bool isMuted = false;  // Flag to check if the audio is muted

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
        if (!isMuted)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayWrongSound()
    {
        if (!isMuted)
        {
            soundEffectSource.PlayOneShot(wrongSound);
        }
    }

    public void PlayCorrectSound()
    {
        if (!isMuted)
        {
            soundEffectSource.PlayOneShot(correctSound);
        }
    }

    public void PlayWinSound()
    {
        if (!isMuted)
        {
            soundEffectSource.PlayOneShot(winSound);
        }
    }

    public void PlayFlipSound()
    {
        if (!isMuted)
        {
            soundEffectSource.PlayOneShot(flipSound);
        }
    }

    public void PlayFailSound()
    {
        if (!isMuted)
        {
            soundEffectSource.PlayOneShot(failSound);
        }
    }

    public void StopAllSound()
    {
        soundEffectSource.Stop();
        musicSource.Stop();
    }

    // Method to toggle mute/unmute
    public void ToggleMute()
    {
        isMuted = !isMuted; 

        if (isMuted)
        {
            musicSource.mute = true;
            soundEffectSource.mute = true;
            muteImg.sprite = muteIcons[0];
        }
        else
        {
            musicSource.mute = false;
            soundEffectSource.mute = false;
            muteImg.sprite = muteIcons[1];
        }
    }
}

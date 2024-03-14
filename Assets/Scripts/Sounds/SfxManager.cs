
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    
    public AudioClip buttonClip,swip,Jump,coin,slide,powerup,notification,laughOnTurn,UpsSfx,GameOver;
    public AudioClip[] death;
    public AudioSource unDuckAudio,sfxAdudio;
    public AudioSource coinAudio;
    AudioSource audioSource;
    public static SfxManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayButtonClick()
    {
        unDuckAudio.clip = buttonClip;
        unDuckAudio.Play();
    }
    public void PlayNotification()
    {
        sfxAdudio.clip = notification;
        sfxAdudio.Play();
    }
    public float progress;
    public float pitchDefault;
    public float pitchMax;

    public void PlayCoinSound()
    {
        coinAudio.Play();
        if(coinAudio.pitch < pitchMax)
        {
            coinAudio.pitch += progress;
        }
        else
        {
            coinAudio.pitch = pitchDefault;
        }
    }
    public void PlaySwipe()
    {
        unDuckAudio.clip = swip;
        unDuckAudio.Play();
    }
    public void PlayJump()
    {
        unDuckAudio.clip = Jump;
        unDuckAudio.Play();
    }
    public void PlaySlide()
    {
        unDuckAudio.clip = slide;
        unDuckAudio.Play();
    }
    public void PlayPowerUp()
    {
        unDuckAudio.clip = powerup;
        unDuckAudio.Play();
    }
    public void PlayLaugh()
    {
        unDuckAudio.clip = laughOnTurn;
        unDuckAudio.Play();
    }
    public void PlayUps()
    {
        sfxAdudio.clip = UpsSfx;
        sfxAdudio.Play();
    }
    public void PlayGameOver()
    {
        sfxAdudio.clip = GameOver;
        sfxAdudio.Play();
    }
    public void PlayCharacterSelectionSound(AudioClip _clip)
    {
        sfxAdudio.clip = _clip;
        sfxAdudio.Play();
    }
    public void PlayDeathSound()
    {
        unDuckAudio.clip = death[Random.Range(0,death.Length)];
        unDuckAudio.Play();
    }
}

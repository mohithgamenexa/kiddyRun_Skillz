
using UnityEngine;

public class footstep : MonoBehaviour
{
    AudioSource mAudio;    
    public AudioClip[] footClips;
    void Start()
    {
        mAudio = GetComponent<AudioSource>();
    }
    public void FootSound()
    {
        AudioClip clip = footClips[Random.Range(0, footClips.Length)];
        mAudio.clip = clip;
        mAudio.Play();
    }
}

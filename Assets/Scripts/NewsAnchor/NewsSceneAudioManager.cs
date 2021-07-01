using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsSceneAudioManager : MonoBehaviour
{
    public static NewsSceneAudioManager instance;

    [SerializeField] private AudioClip tvOnAudioClip;
    [SerializeField] private AudioClip introStepsAudioClip;
    [SerializeField] private AudioClip bgMusicAudioClip;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("More than one instance of NewsSceneAudioManager found");
            Destroy(this);
        }
    }

    private void Start()
    {
        musicAudioSource.clip = bgMusicAudioClip;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayTVOnSFX()
    {
        sfxAudioSource.Stop();
        sfxAudioSource.clip = tvOnAudioClip;
        sfxAudioSource.Play();
    }

    public void PlayIntroSteps()
    {
        sfxAudioSource.PlayOneShot(introStepsAudioClip);
    }
}

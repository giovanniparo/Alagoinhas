using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClip defaultBgAudioClip;
    [SerializeField] private AudioClip[] walkAudioClips;
    [SerializeField] private AudioClip monsterGrowlAudioClip;
    [SerializeField] private AudioClip windAudioClip;
    [SerializeField] private AudioClip waterfallAudioClip;
    [SerializeField] private AudioClip creepViolinAudioClip;
    [SerializeField] private AudioClip lanternSwitchAudioClip;
    [SerializeField] private AudioClip doorAudioClip;
    [SerializeField] private AudioClip gameOverAudioClip;
    [SerializeField] private AudioClip bloodAudioClip;
    [SerializeField] private AudioClip howlingAudioClip;
    [SerializeField] private AudioClip birdAudioClip;
    [SerializeField] private AudioClip pickItemSound;

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource walkSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("More than one instance of SoundManager found!");
            Destroy(this);
        }
    }

    private void Start()
    {
        musicSource.clip = defaultBgAudioClip;
        PlayLevelMusic();
    }

    public void PlayWalkSFX()
    {
        float randomWalk = Random.value;
        if (!walkSource.isPlaying)
        {
            if(randomWalk <= 0.8f)
                walkSource.clip = walkAudioClips[Random.Range(0, 1)];
            else if(randomWalk <= 0.95f)
                walkSource.clip = walkAudioClips[2];
            else
                walkSource.clip = walkAudioClips[3];
            walkSource.Play();
        }
    }

    public void PlayLevelMusic()
    {
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopLevelMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(string audioName)
    {
        switch (audioName)
        {
            case "MonsterGrowl":
                sfxSource.volume = 1.0f;
                sfxSource.PlayOneShot(monsterGrowlAudioClip);
                break;
            case "Violin":
                sfxSource.volume = 1.0f;
                sfxSource.PlayOneShot(creepViolinAudioClip);
                break;
            case "Lantern":
                sfxSource.volume = 1.0f;
                sfxSource.PlayOneShot(lanternSwitchAudioClip);
                break;
            case "Door":
                sfxSource.volume = 0.8f;
                sfxSource.PlayOneShot(doorAudioClip);
                break;
            case "GameOver":
                sfxSource.volume = 1.0f;
                sfxSource.PlayOneShot(gameOverAudioClip);
                break;
            case "Blood":
                sfxSource.volume = 1.0f;
                sfxSource.PlayOneShot(bloodAudioClip);
                break;
            case "Howl":
                sfxSource.volume = 1.0f;
                sfxSource.PlayOneShot(howlingAudioClip);
                break;
            case "Bird":
                sfxSource.volume = 0.7f;
                sfxSource.PlayOneShot(birdAudioClip);
                break;
            case "Item":
                sfxSource.volume = 0.8f;
                sfxSource.PlayOneShot(pickItemSound);
                break;
        }
    }

    public bool IsSFXPlaying()
    {
        return sfxSource.isPlaying;
    }
}

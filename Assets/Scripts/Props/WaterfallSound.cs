using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallSound : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    
    private GameObject player;
    private AudioSource selfAudioSource;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        selfAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        selfAudioSource.volume =  Mathf.Clamp01(1.0f - (maxDistance / (transform.position - player.transform.position).magnitude));
    }
}

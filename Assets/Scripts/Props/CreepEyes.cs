using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepEyes : MonoBehaviour
{
    [SerializeField] private float blinkSpeed;
    [SerializeField] private float blinkFrequency;
    [SerializeField] private float frequencyAmplitude;

    private float blinkTimer = 0.0f;
    private float blinkSpeedTimer = 0.0f;
    private float nextBlink = 0.0f;

    private SpriteRenderer spriteRenderer;

    private bool blinking = false;

    private void Start()
    {
        nextBlink = GetNextBlink();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(blinkTimer >= nextBlink && !blinking)
        {
            nextBlink = GetNextBlink();
            blinkTimer = 0.0f;
            spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
            blinking = true;
        }

        if (blinking)
        {
            if(blinkSpeedTimer >= blinkSpeed)
            {
                spriteRenderer.color = new Color(1f, 0f, 0f, 0f);
                blinkSpeedTimer = 0.0f;
                blinking = false;
            }

            blinkSpeedTimer += Time.deltaTime;
        }
        else
            blinkTimer += Time.deltaTime;
    }

    private float GetNextBlink()
    {
        float freqAmp = Random.Range(0.0f, frequencyAmplitude);
        return Random.Range(1.0f / blinkFrequency + freqAmp, 1.0f / blinkFrequency - freqAmp);
    }
}

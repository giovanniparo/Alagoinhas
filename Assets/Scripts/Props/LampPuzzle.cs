using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject lampLever;
    [SerializeField] private GameObject lampFire;
    [SerializeField] private float resetTime = 2.0f;

    [SerializeField] private LampPuzzleController lampPuzzleController;

    public bool pull = false;
    public bool done = false;
    public bool waiting = false;
    public bool reseting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!pull && !waiting && other.CompareTag("Player"))
        {
            SpeechManager.instance.PlayEmote(other.transform, EmoteTypes.Exclama);
            StartCoroutine(WaitForInputCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpeechManager.instance.ClearEmote();
            StopCoroutine(WaitForInputCoroutine());
        }
    }

    private void Update()
    {
        lampLever.GetComponent<LampLever>().pull = pull;
    }

    private IEnumerator WaitForInputCoroutine()
    {
        waiting = true;
        while (!Input.GetKeyDown(KeyCode.E))
            yield return null;

        SoundManager.instance.PlaySFX("Lanterna");
        lampPuzzleController.LeverWasPulled(this);
        SpeechManager.instance.ClearEmote();
        pull = true;
        lampFire.SetActive(true);
        waiting = false;
    }

    public void ResetLamp()
    {
        if(!reseting)
            StartCoroutine(ResetLampCoroutine());
    }

    IEnumerator ResetLampCoroutine()
    {
        reseting = true;
        yield return new WaitForSeconds(resetTime);
        SoundManager.instance.PlaySFX("Lanterna");
        lampFire.SetActive(false);
        pull = false;
        reseting = false;
    }
}

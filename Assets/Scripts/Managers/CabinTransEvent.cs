using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinTransEvent : MonoBehaviour
{
    [SerializeField] private int indexSceneToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        { 
            SpeechManager.instance.PlayEmote(other.gameObject.transform, EmoteTypes.Exclama);
            StartCoroutine(WaitingForInputCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpeechManager.instance.ClearEmote();
            StopCoroutine(WaitingForInputCoroutine());
        }
    }

    IEnumerator WaitingForInputCoroutine()
    {
        while (!Input.GetKeyDown(KeyCode.E))
            yield return null;

        SoundManager.instance.PlaySFX("Door");
        SceneLoader.instance.LoadScene(indexSceneToLoad);
    }
}

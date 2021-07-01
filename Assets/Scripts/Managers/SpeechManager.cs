using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmoteTypes
{
    Exclama,
    Interroga
}

public class SpeechManager : MonoBehaviour
{
    public static SpeechManager instance;

    [SerializeField] private Vector3 speechBubblePosition;
    [SerializeField] private Vector2 emoteBubbleOffset;
    [SerializeField] private GameObject speechBubblePrefab;
    [SerializeField] private GameObject[] emotePrefabs; //0  - Exclama 1- Interroga

    private GameObject playedEmote;
    public SpeechBubble currentSpeechBubbleScript = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance of SpeechManager found");
            Destroy(this);
        }
        else
            instance = this;
    }

    public void Create(Transform parent, Vector3 localPosition, string text)
    {
        GameObject speechBubbleObject = Instantiate(speechBubblePrefab, parent);
        speechBubbleObject.transform.localPosition = localPosition;

        currentSpeechBubbleScript = speechBubbleObject.GetComponent<SpeechBubble>();
        if(currentSpeechBubbleScript != null)
            currentSpeechBubbleScript.Setup(text, 0.1f);
    }

    public void PlayEmote(Transform parent, EmoteTypes emoteType)
    {
        if(currentSpeechBubbleScript == null)
        {
            playedEmote = Instantiate(emotePrefabs[(int)emoteType], parent);
            playedEmote.transform.localPosition = emoteBubbleOffset;
        }
    }

    public void ClearEmote()
    {
        if(playedEmote != null)
            Destroy(playedEmote);
    }
}

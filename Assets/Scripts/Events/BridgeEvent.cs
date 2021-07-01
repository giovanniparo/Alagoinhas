using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeEvent : MonoBehaviour
{
    [SerializeField] private Item bridgeEventItemFlag;
    private GameObject playerGameObject;

    private bool running = false;

    private void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!running && other.CompareTag("Player") && !Inventory.instance.CheckInventory(bridgeEventItemFlag.itemName))
        {
            StartCoroutine(TriggerEventCoroutine());
        }
    }

    private IEnumerator TriggerEventCoroutine()
    {
        running = true;
        Inventory.instance.AddItem(bridgeEventItemFlag);
        GameManager.instance.SetGameBusy(true);
        playerGameObject.GetComponent<Player>().isBusy = true;

        playerGameObject.GetComponent<Player>().StopMovement();
        SpeechManager.instance.PlayEmote(playerGameObject.transform, EmoteTypes.Exclama);
        yield return new WaitForSeconds(2.0f);
        SpeechManager.instance.ClearEmote();

        yield return new WaitForSeconds(1.0f);
        SpeechManager.instance.Create(playerGameObject.transform, new Vector3(0.0f, 1.75f, 0.0f), "Agora eu lembro...");
        while (SpeechManager.instance.currentSpeechBubbleScript != null)
            yield return null;
        SpeechManager.instance.Create(playerGameObject.transform, new Vector3(0.0f, 1.75f, 0), "Foi ele! Meu namorado!");
        while (SpeechManager.instance.currentSpeechBubbleScript != null)
            yield return null;
        SpeechManager.instance.Create(playerGameObject.transform, new Vector3(0.0f, 1.75f, 0), "Ele me jogou aqui nesse mato.");
        while (SpeechManager.instance.currentSpeechBubbleScript != null)
            yield return null;
        yield return new WaitForSeconds(1.0f);

        playerGameObject.GetComponent<Player>().isBusy = false;
        GameManager.instance.SetGameBusy(false);
        running = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntroEvent : MonoBehaviour
{
    [SerializeField] private Item playerIntroEventItemFlag;
    private GameObject playerGameObject;

    [SerializeField] private float wakeUpTime = 4.0f;

    [SerializeField] private Transform[] playerMoveTargets;

    private bool running = false;

    private void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (running == false && !Inventory.instance.CheckInventory(playerIntroEventItemFlag.itemName))
        {
            StartCoroutine(TriggerEventCoroutine());
        }
    }

    private IEnumerator TriggerEventCoroutine()
    {
        running = true;
        int nCounter = 0;
        Inventory.instance.AddItem(playerIntroEventItemFlag);
        GameManager.instance.SetGameBusy(true);
        playerGameObject.GetComponent<Player>().isBusy = true;

        playerGameObject.GetComponent<Player>().StopMovement();
        SpeechManager.instance.PlayEmote(playerGameObject.transform, EmoteTypes.Interroga);
        yield return new WaitForSeconds(wakeUpTime);

        while (nCounter < playerMoveTargets.Length)
        {
            playerGameObject.GetComponent<Player>().MoveFromScript(playerMoveTargets[nCounter].position, 5.0f);
            while (!playerGameObject.GetComponent<Player>().onTarget)
                yield return null;
            nCounter++;
        }
        SpeechManager.instance.ClearEmote();

        SpeechManager.instance.Create(playerGameObject.transform, new Vector3(0.0f, 1.75f, 0.0f), "Onde é que eu tô?");
        while (SpeechManager.instance.currentSpeechBubbleScript != null)
            yield return null;
        SpeechManager.instance.Create(transform, new Vector3(0.0f, 1.75f, 0), "Será que estou em Alagoinha?");
        while (SpeechManager.instance.currentSpeechBubbleScript != null)
            yield return null;
        SpeechManager.instance.Create(transform, new Vector3(0.0f, 1.75f, 0), "Será que foi na selva?");
        while (SpeechManager.instance.currentSpeechBubbleScript != null)
            yield return null;
        SoundManager.instance.PlaySFX("Howl");
        SpeechManager.instance.PlayEmote(playerGameObject.transform, EmoteTypes.Exclama);
        while (SoundManager.instance.IsSFXPlaying())
            yield return null;
        SpeechManager.instance.ClearEmote();
        SpeechManager.instance.Create(transform, new Vector3(0.0f, 1.75f, 0), "Será que tem algum animal por aqui?");
        while (SpeechManager.instance.currentSpeechBubbleScript != null)
            yield return null;
        SpeechManager.instance.Create(transform, new Vector3(0.0f, 1.75f, 0), "Preciso encontrar um jeito de sair daqui!");
        while (SpeechManager.instance.currentSpeechBubbleScript != null)
            yield return null;
        playerGameObject.GetComponent<Player>().isBusy = false;
        GameManager.instance.SetGameBusy(false);
        running = false;
    }
}

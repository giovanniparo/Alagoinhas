using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIntroEvent : MonoBehaviour
{
    [SerializeField] private GameObject enemyEventMonsterPrefab;
    [SerializeField] private Item introEventItemFlag;
    private GameObject playerGameObject;

    [SerializeField] private Transform[] enemyMoveTargets;
    [SerializeField] private Transform[] playerMoveTargetsBefore;
    [SerializeField] private Transform[] playerMoveTargetsAfter;

    private bool running = false;

    private void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(Inventory.instance.CheckInventory(introEventItemFlag.itemName));
        if (other.CompareTag("Player") && !Inventory.instance.CheckInventory(introEventItemFlag.itemName) && !running)
        {
            Debug.Log("TriggerInside");
            StartCoroutine(TriggerEventCoroutine());
        }
    }

    IEnumerator TriggerEventCoroutine()
    {
        running = true;
        int nCounter = 0;
        Inventory.instance.AddItem(introEventItemFlag);
        GameManager.instance.SetGameBusy(true);

        playerGameObject.GetComponent<Player>().StopMovement();
        SoundManager.instance.PlaySFX("MonsterGrowl");
        SpeechManager.instance.PlayEmote(playerGameObject.transform, EmoteTypes.Exclama);
        while (SoundManager.instance.IsSFXPlaying())
            yield return null;
        while(nCounter < playerMoveTargetsBefore.Length)
        {
            playerGameObject.GetComponent<Player>().MoveFromScript(playerMoveTargetsBefore[nCounter].position, 7.5f);
            while (!playerGameObject.GetComponent<Player>().onTarget)
                yield return null;
            nCounter++;
        }
        SpeechManager.instance.ClearEmote();

        nCounter = 0;
        GameObject enemyEvent = Instantiate(enemyEventMonsterPrefab, enemyMoveTargets[nCounter].position, Quaternion.identity);
        EnemyEventsMonster enemyEventScript = enemyEvent.GetComponent<EnemyEventsMonster>();
        nCounter++;
        if (enemyEventScript != null)
        {
            while(nCounter < enemyMoveTargets.Length)
            {
                enemyEventScript.OrderToMove(enemyMoveTargets[nCounter].position, 2.5f);
                while (!enemyEventScript.onTarget)
                    yield return null;
                nCounter++;
            }
        }

        Destroy(enemyEvent);

        nCounter = 0;
        while (nCounter < playerMoveTargetsAfter.Length)
        {
            playerGameObject.GetComponent<Player>().MoveFromScript(playerMoveTargetsAfter[nCounter].position, 7.5f);
            while (!playerGameObject.GetComponent<Player>().onTarget)
                yield return null;
            nCounter++;
        }

        playerGameObject.GetComponent<Player>().LookTo(Direction.Right);
        SpeechManager.instance.Create(playerGameObject.transform, new Vector3(0.0f, 1.75f, 0.0f), "O que era aquilo?");
        GameManager.instance.SetGameBusy(false);
        running = false;
    }
}

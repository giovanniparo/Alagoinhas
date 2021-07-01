using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item pickableItem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpeechManager.instance.PlayEmote(other.gameObject.transform, EmoteTypes.Exclama);
            other.GetComponent<Player>().SetPickUp(true, this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpeechManager.instance.ClearEmote();
            other.GetComponent<Player>().SetPickUp(false);
        }
    }

    public void ItemPicked()
    {
        Destroy(this.gameObject);
    }
}

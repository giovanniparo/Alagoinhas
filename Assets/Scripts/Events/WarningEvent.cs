using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningEvent : MonoBehaviour
{
    public Item warningEventItem;
    private bool init = false;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }


    private void LateUpdate()
    {
        if(!init && !Inventory.instance.CheckInventory(warningEventItem.itemName))
        {
            init = true;
            Inventory.instance.AddItem(warningEventItem);
            player.isBusy = true;
            GameManager.instance.SetGameBusy(true);
            UIManager.instance.ShowWarning(true);
        }
    }
}

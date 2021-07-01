using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    public Item[] cheatItems;

    void Start()
    {
        foreach(Item it in cheatItems)
        {
            Inventory.instance.AddItem(it);
        }
    }
}

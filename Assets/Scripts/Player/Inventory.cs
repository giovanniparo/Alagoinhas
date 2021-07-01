using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private List<Item> inventory = new List<Item>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("More than one instance of Inventory was found!");
            Destroy(this);
        }
    }

    public void AddItem(Item toAdd)
    {
        inventory.Add(toAdd);
        ShowInventory();
    }

    public void RemoveItem(Item toRemove)
    {
        if (inventory.Remove(toRemove))
        {
            Debug.Log("Item " + toRemove + " was removed");
        }
        ShowInventory();
    }

    public bool CheckInventory(string itemName)
    {
        foreach(Item itm in inventory)
        {
            if (itm.itemName == itemName)
                return true;
        }

        return false;
    }

    private void ShowInventory()
    {
        foreach(Item itm in inventory)
        {
            Debug.Log(itm.name);
        }
    }

}

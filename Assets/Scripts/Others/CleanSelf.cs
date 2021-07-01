using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pickup))]
public class CleanSelf : MonoBehaviour
{
    private Pickup pickUp;

    void Start()
    {
        pickUp = GetComponent<Pickup>();

        if (Inventory.instance.CheckInventory(pickUp.pickableItem.itemName))
            Destroy(this.gameObject);
    }

}

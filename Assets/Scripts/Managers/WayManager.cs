using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayManager : MonoBehaviour
{
    [SerializeField] private GameObject fogWallTop;
    [SerializeField] private GameObject fogWallLeft;
    [SerializeField] private GameObject fogWallBot;
    [SerializeField] private GameObject transEventTop;
    [SerializeField] private GameObject transEventLeft;
    [SerializeField] private GameObject transEventBot;

    private void Start()
    {
        if (Inventory.instance.CheckInventory("CheckedCar"))
        {
            fogWallBot.SetActive(false);
            transEventBot.SetActive(true);

            if (Inventory.instance.CheckInventory("Bateria"))
            {
                fogWallLeft.SetActive(false);
                transEventLeft.SetActive(true);

                if (Inventory.instance.CheckInventory("Chaves"))
                {
                    fogWallTop.SetActive(false);
                    transEventTop.SetActive(true);
                }
            }
        }
    }
}

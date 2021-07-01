using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampPuzzleController : MonoBehaviour
{
    private List<LampPuzzle> triedList = new List<LampPuzzle>();
    [SerializeField] private LampPuzzle[] lampScripts;
    [SerializeField] private GameObject gasolineKey;

    private int currentTryingIndex = 0;

    private void Start()
    {
        gasolineKey.SetActive(false);

        if (!Inventory.instance.CheckInventory("Gasolina"))
        {
            foreach (LampPuzzle puzzScript in lampScripts)
            {
                puzzScript.ResetLamp();
            }
        }
    }

    public void LeverWasPulled(LampPuzzle pulledLever)
    {
        triedList.Add(pulledLever);

        if (pulledLever != lampScripts[currentTryingIndex])
        {
            foreach (LampPuzzle lampPuzzle in triedList)
                lampPuzzle.ResetLamp();
            triedList.Clear();
            currentTryingIndex = 0;
            return;
        }
        else
            currentTryingIndex++;

        if (currentTryingIndex == lampScripts.Length)
            gasolineKey.SetActive(true);
    }
}

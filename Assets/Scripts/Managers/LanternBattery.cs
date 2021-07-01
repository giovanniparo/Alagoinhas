using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanternBattery : MonoBehaviour
{
    public Image lanternBatteryMaskImage;
    public int numOfSteps;

    private float finalMaskWidth;
    private float stepInc;

    private void Start()
    {
        finalMaskWidth = lanternBatteryMaskImage.rectTransform.sizeDelta.x;
        stepInc = finalMaskWidth / numOfSteps;
    }

    public void UpdateLanternBattery(float currentBatteryPercentage)
    {
        float percentagePerStep = 100.0f / numOfSteps;
        lanternBatteryMaskImage.rectTransform.sizeDelta = new Vector2(((100.0f - currentBatteryPercentage) / percentagePerStep) * stepInc,
                                                                      lanternBatteryMaskImage.rectTransform.sizeDelta.y);
    }
}
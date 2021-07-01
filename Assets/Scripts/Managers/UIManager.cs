using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject warningPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject objectivesUI;
    [SerializeField] private GameObject lanternUI;
    [SerializeField] private GameObject carBatteryUI;
    [SerializeField] private GameObject carKeysUI;
    [SerializeField] private GameObject carGasUI;
    [SerializeField] private Sprite[] completeSprites;

    [SerializeField] private LanternBattery lanternBatteryUI;
    [SerializeField] private Image transitionMask;
    [SerializeField] private float fadeTime;

    [HideInInspector] public bool fadingIn = false;
    [HideInInspector] public bool fadingOut = false;

    public void SetGameOverScreen(bool gameOverState)
    {
        transitionMask.gameObject.SetActive(!gameOverState);
        objectivesUI.SetActive(!gameOverState);
        lanternUI.SetActive(!gameOverState);
        carBatteryUI.SetActive(!gameOverState);
        carKeysUI.SetActive(!gameOverState);
        carGasUI.SetActive(!gameOverState);
        endGamePanel.SetActive(!gameOverState);
        gameOverPanel.SetActive(gameOverState);
    }

    public void SetEndGameScreen(bool endGameState)
    {
        transitionMask.gameObject.SetActive(!endGameState);
        objectivesUI.SetActive(!endGameState);
        lanternUI.SetActive(!endGameState);
        carBatteryUI.SetActive(!endGameState);
        carKeysUI.SetActive(!endGameState);
        carGasUI.SetActive(!endGameState);
        gameOverPanel.SetActive(!endGameState);
        endGamePanel.SetActive(endGameState);
    }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one instance of UIManager found!");
            Destroy(this);
        }

    }

    private void Start()
    {
        transitionMask.color = Color.black;
        ClearUI();
        FadeIn();
    }

    private void Update()
    {
        if (fadingIn)
        {
            transitionMask.color -= new Color(0f, 0f, 0f, fadeTime * Time.deltaTime);
            if(Mathf.Abs(transitionMask.color.a) <= 0.1f)
                DoneFading();
        }
        else if (fadingOut)
        {
            transitionMask.color += new Color(0f, 0f, 0f, fadeTime * Time.deltaTime);
            if (Mathf.Abs(transitionMask.color.a - 1.0f) <= 0.1f)
                DoneFading();
        }
    }

    public void DoneFading()
    {
        fadingIn = false;
        fadingOut = false;
        GameManager.instance.SetGameBusy(false);
    }

    public void FadeIn()
    {
        fadingIn = true;
        fadingOut = false;
        GameManager.instance.SetGameBusy(true);
    }

    public void FadeOut()
    {
        fadingOut = true;
        fadingIn = false;
        GameManager.instance.SetGameBusy(true);
    }
    
    public void UpdateLanternBatteryUI(float currentBatteryPercentage)
    {
        lanternBatteryUI.UpdateLanternBattery(currentBatteryPercentage);
    }

    public void UpdateItemUIImages(Item addedItem)
    {
        switch (addedItem.itemName)
        {
            case "Lanterna":
                lanternUI.SetActive(true);
                UpdateLanternBatteryUI(100.0f);
                break;
            case "Bateria":
                carBatteryUI.GetComponent<Image>().sprite = completeSprites[1];
                break;
            case "Chaves":
                carKeysUI.GetComponent<Image>().sprite = completeSprites[1];
                break;
            case "Gasolina":
                carGasUI.GetComponent<Image>().sprite = completeSprites[1];
                break;
        }
    }

    public void ClearUI()
    {
        lanternUI.SetActive(false);
        carBatteryUI.GetComponent<Image>().sprite = completeSprites[0];
        carKeysUI.GetComponent<Image>().sprite = completeSprites[0];
        carGasUI.GetComponent<Image>().sprite = completeSprites[0];
        gameOverPanel.SetActive(false);
        endGamePanel.SetActive(false);
        objectivesUI.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void ShowControls(bool state)
    {
        controlsPanel.SetActive(state);
    }

    public void ShowObjectives()
    {
        objectivesUI.SetActive(true);
    }

    public void ShowWarning(bool state)
    {
        Cursor.visible = true;
        warningPanel.SetActive(state);
    }
}

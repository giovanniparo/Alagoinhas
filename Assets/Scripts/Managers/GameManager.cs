using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float batteryLife = 180.0f;
    [SerializeField] private GameObject canvasObj;

    private float batteryTimer;
    private bool gameBusy = false;
    private bool init = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("More than one instance of GameManager found");
            Destroy(this);
        }

        DontDestroyOnLoad(this.gameObject);
        ResetLanternBattery();
    }

    private void LateUpdate()
    {
        if (!init)
        {
            init = true;
            UIManager.instance.ShowControls(true);
        }
    }

    public void ResetLanternBattery()
    {
        batteryTimer = batteryLife;
    }

    public void UpdateBatteryTimer(float elapsedTime)
    {
        batteryTimer -= elapsedTime;
    }

    public float GetBatteryPercentage()
    {
        return 100.0f * (batteryTimer / batteryLife);
    }

    public void SetGameBusy(bool gameBusy)
    {
        this.gameBusy = gameBusy;
    }

    public bool GetGameBusy()
    {
        return gameBusy;
    }

    public void GameOver()
    {
        SetGameBusy(true);

        GameObject.FindGameObjectWithTag("Player").SetActive(false);
        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(activeEnemies != null && activeEnemies.Length > 0)
        {
            foreach(GameObject enemy in activeEnemies)
            {
                enemy.SetActive(false);
            }
        }

        Cursor.visible = true;
        UIManager.instance.SetGameOverScreen(true);
    }

    public void EndGame()
    {
        SetGameBusy(true);

        GameObject.FindGameObjectWithTag("Player").SetActive(false);
        Cursor.visible = true;
        UIManager.instance.SetEndGameScreen(true);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void CleanSelf()
    { 
        Destroy(canvasObj);
        Destroy(this.gameObject);
    }

    public void ControlsContinueButton()
    {
        Cursor.visible = false;
        UIManager.instance.ShowControls(false);
        SceneLoader.instance.LoadScene(3);
    }

    public void WarningContinueButton()
    {
        Cursor.visible = false;
        UIManager.instance.ShowWarning(false);
        SetGameBusy(false);
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.isBusy = false;
    }
}

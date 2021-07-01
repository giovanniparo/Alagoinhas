using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    public int currentMapIndex;
    public int lastMapIndex;

    private bool loading = false;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("More than one instance of SceneManager found");
            Destroy(this);
        }
    }
    public void LoadScene(int sceneIndex)
    {
        if(!loading)
            StartCoroutine(LoadSceneCoroutine(sceneIndex));
    }
        
    public IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        loading = true;
        Scene currentScene;

        UIManager.instance.FadeOut();
        while (UIManager.instance.fadingOut)
            yield return null;

        currentScene = SceneManager.GetActiveScene();
        lastMapIndex = currentScene.buildIndex;
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        currentScene = SceneManager.GetActiveScene();
        currentMapIndex = currentScene.buildIndex;

        UIManager.instance.FadeIn();
        while (UIManager.instance.fadingIn)
            yield return null;

        loading = false;
    }

    public void ResetGame()
    {
        UIManager.instance.ClearUI();
        SceneManager.LoadScene(2);
        GameManager.instance.CleanSelf();
    }
}

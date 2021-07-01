using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewsSceneLoader : MonoBehaviour
{
    public static NewsSceneLoader instance;

    [SerializeField] private float fadeTime;
    [SerializeField] private Image transitionMask;

    private bool loading = false;
    private bool fadingIn = false;
    private bool fadingOut = false;
    public bool isBusy = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("More than one instance of NewsSceneManager found");
            Destroy(this);
        }
    }

    private void Update()
    {
        if (fadingIn)
        {
            transitionMask.color -= new Color(0f, 0f, 0f, fadeTime * Time.deltaTime);
            if (Mathf.Abs(transitionMask.color.a) <= 0.1f)
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
        isBusy = false;
    }

    public void FadeIn()
    {
        fadingIn = true;
        fadingOut = false;
        isBusy = true;
    }

    public void FadeOut()
    {
        fadingOut = true;
        fadingIn = false;
        isBusy = true;
    }

    public void LoadScene(int sceneIndex)
    {
        if (!loading)
            StartCoroutine(LoadSceneCoroutine(sceneIndex));
    }

    public IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        loading = true;
        FadeOut();

        while (fadingOut)
            yield return null;

        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        loading = false;
    }

}

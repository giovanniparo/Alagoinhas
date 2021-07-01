using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransEvent : MonoBehaviour
{
    [SerializeField] private int indexSceneToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.instance.LoadScene(indexSceneToLoad);
        }
    }
}

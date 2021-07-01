using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PositionPlayerController : MonoBehaviour
{
    [SerializeField] private Transform[] playerInitPositions;
    [SerializeField] private int[] relatedMapIndices;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if(relatedMapIndices.Length != playerInitPositions.Length)
        {
            Debug.LogError("Error: Different number of map index to playerInitPositions on " + SceneManager.GetActiveScene().name);
        }
        
        for(int n = 0; n < relatedMapIndices.Length; n++)
        {
            if (SceneLoader.instance.lastMapIndex == relatedMapIndices[n])
            {
                player.transform.position = playerInitPositions[n].position;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    private Player player;
    private Enemy enemy;
    private LayerMask losLayer;
    private bool onLos = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player == null)
            Debug.LogError("No player found on scene for LoS");
        enemy = GetComponentInParent<Enemy>();
        losLayer = LayerMask.GetMask("los");
    }

    private void Update()
    {
        if (onLos &&
            player.lanternOn &&
            Mathf.Approximately(Vector3.Dot(transform.up, player.currentLightDirection), -1.0f))
        {
            enemy.StunEnemy();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && 
            !Physics2D.Raycast(transform.position, other.transform.position - transform.position, (other.transform.position - transform.position).magnitude + 1.0f, losLayer))
        {
            onLos = true;
            enemy.onLoS = true;
            enemy.AttackPlayer(other.transform);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") &&
            !Physics2D.Raycast(transform.position, other.transform.position - transform.position, (other.transform.position - transform.position).magnitude + 1.0f, losLayer))
        {
            onLos = true;
            enemy.onLoS = true;
            enemy.AttackPlayer(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onLos = false;
            enemy.onLoS = false;
        }
    }
}

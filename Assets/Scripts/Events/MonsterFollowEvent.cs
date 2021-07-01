using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFollowEvent : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesOnMap;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private Transform monsterInitPos;
    [SerializeField] private Transform[] monsterGotoPositions;

    private GameObject followEnemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Inventory.instance.CheckInventory("Chaves"))
        {
            foreach(GameObject enemyOnMap in enemiesOnMap)
            {
                enemyOnMap.SetActive(false);
            }

            followEnemy = Instantiate(enemyPrefab, monsterInitPos.position, Quaternion.identity);
            followEnemy.GetComponent<Enemy>().SetEnemyState(Enemy.EnemyStates.Patrol, monsterGotoPositions, false);

            gameObject.SetActive(false);
        }
    }
}

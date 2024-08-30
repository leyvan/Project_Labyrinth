using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACEnemySpawner : MonoBehaviour
{
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private GameObject enemyToSpawn;
    private Transform thisSpawnerTransform;

    private Vector3 enemySpawnOffset;
    private Vector3 newEnemyPosition;
    private void Start()
    {
        enemySpawnOffset = new Vector3(-1f, enemyToSpawn.transform.localScale.y / 2, numberOfEnemies * -1);
        thisSpawnerTransform = this.transform;
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (numberOfEnemies != 0)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                newEnemyPosition = thisSpawnerTransform.position + enemySpawnOffset;
                GameObject newEnemy = Instantiate(enemyToSpawn, newEnemyPosition, thisSpawnerTransform.rotation, thisSpawnerTransform);

                enemySpawnOffset = enemySpawnOffset + new Vector3(0,0,2f);
            }
        }
    }
}

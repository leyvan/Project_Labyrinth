using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBehavior : MonoBehaviour
{
    public List<Transform> itemSpawnPoints = new List<Transform>();
    public List<Transform> enemySpawnPoints = new List<Transform>();

    void Awake()
    {
        GetItemSpawnPoints();
        GetEnemySpawnPoints();
    }

    private void GetItemSpawnPoints()
    {
        var maxChildCount = this.gameObject.transform.childCount;
        var itemSpawnsParent = this.gameObject.transform.GetChild(maxChildCount-1);
        foreach(Transform child in itemSpawnsParent)
        {
            itemSpawnPoints.Add(child);
        }

        RandomlySpawnItems();
    }

    void GetEnemySpawnPoints()
    {
        var enemySpawnParent = this.gameObject.transform.GetChild(4);

        if(enemySpawnParent != null)
        {
            foreach(Transform child in enemySpawnParent)
            {
                enemySpawnPoints.Add(child);
            }

            RandomlySpawnEnemies();
        }
    }

    void RandomlySpawnItems()
    {
        
        Object[] items = Resources.LoadAll("Prefabs/Items", typeof(GameObject)) ;
        
        foreach(Transform spawnPoint in itemSpawnPoints)
        {
            var rand = Random.Range(0, 5);
            Instantiate(items[rand], spawnPoint.position + new Vector3(0,1,0), spawnPoint.rotation * Quaternion.Euler(90, 0, 0));
        }
    }

    void RandomlySpawnEnemies()
    {
        GameObject enemy = Resources.Load("Prefabs/Misc/Enemy_AI_V1") as GameObject;

        foreach(Transform spawnPoint in enemySpawnPoints)
        {
            var rand = Random.Range(0, 2);      // 50/50 chance to spawn enemy
            if(rand == 1)
            Instantiate(enemy, spawnPoint);
        }
    }
}
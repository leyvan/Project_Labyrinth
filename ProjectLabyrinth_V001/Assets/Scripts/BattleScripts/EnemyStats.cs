using System;
using System.Collections;
using System.Collections.Generic;
using CombatUtil;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] public BaseEnemy thisEnemy;
    [SerializeField] public EnemyConfigScriptableObject enemyConfig;
    [Header("Its a one in this number(-1) chance to drop an item. In other words (1/x-1) since Max is Exclusive")]
    public int dropChance = 1;
    
    public void SetEnemyStats(EnemyType enemyType)
    {
        thisEnemy = new BaseEnemy(enemyConfig.GetEnemyFromType(enemyType));
    }

    public int GetXPDropped()
    {
        return Random.Range(thisEnemy.earnableXPRange[0], thisEnemy.earnableXPRange[1]);
    }

    public int GetGoldDropped()
    {
        return Random.Range(thisEnemy.earnableGoldRange[0], thisEnemy.earnableGoldRange[1]);
    }

    public BaseItem GetItemDropped()
    {
        BaseItem itemDropped = null;
        int itemDropChance = Random.Range(0, dropChance);

        if (itemDropChance == 0)
        {
            int itemIndex = Random.Range(0, thisEnemy.earnableItems.Count);
            itemDropped = thisEnemy.earnableItems[itemIndex];
        }
        
        return itemDropped;
    }
}

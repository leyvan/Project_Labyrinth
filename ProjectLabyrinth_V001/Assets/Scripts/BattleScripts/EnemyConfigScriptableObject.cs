using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatUtil;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Combat/EnemyConfig", order = 13)]
public class EnemyConfigScriptableObject : ScriptableObject
{
    public List<SpawnableEnemies> enemies = new List<SpawnableEnemies>();

    public BaseEnemy GetEnemyFromType(EnemyType enemyType)
    {
        return enemies.Find((x) => x.enemyType == enemyType).enemyData;
    }

    public GameObject GetEnemyPrefabFromType(EnemyType type)
    {
        return enemies.Find((x) => x.enemyType == type).enemyPrefab;
    }
}

[System.Serializable]
public class SpawnableEnemies
{
    public EnemyType enemyType;
    
    [Header("Enemy Config")]
    public BaseEnemy enemyData;

    [Header("Enemy Prefab")]
    public GameObject enemyPrefab;
}




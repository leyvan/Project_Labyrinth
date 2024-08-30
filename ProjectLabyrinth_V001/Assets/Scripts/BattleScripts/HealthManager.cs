using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;
    private GameObject player;
    private Player_Behaviour playerBehaviour;

    private float playerCurrentHealth;

    private void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerBehaviour = player.GetComponent<Player_Behaviour>();
        playerCurrentHealth = playerBehaviour.mainCharacterStats.curHealth;
    }

    
    public void SetPersistentHealth(float newHealth)
    {
        playerCurrentHealth = newHealth;
    }
    
    public float GetPersistentHealth()
    {
        return playerCurrentHealth;
    }
    
}

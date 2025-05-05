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

        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
            
        
        player = GameObject.FindGameObjectWithTag("Player");
        playerBehaviour = player.GetComponent<Player_Behaviour>();
        playerCurrentHealth = playerBehaviour.mainCharacterStats.maxHealth;
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

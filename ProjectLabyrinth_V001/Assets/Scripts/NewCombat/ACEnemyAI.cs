using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACEnemyAI : MonoBehaviour
{
    private float health;
    private float damage;
    private float attackSpeed;
    private float defense;

    private enum ElementType
    {
        Fire,
        Water,
        Air,
        Light,
        Dark
    };

    private float attackRadius;
    [SerializeField] private float moveSpeed = 0.5f;
    
    private bool playerIsInRange = false;
    private bool startToMove = false;
    private bool canMove = true;
    
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        MoveToPlayer();
    }

    private void Update()
    {
        if (startToMove = true && canMove == true)
        {
            MoveEnemy();
        }
    }

    private void MoveToPlayer()
    {
        if (player != null)
        {
            Debug.Log("Success");
            startToMove = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsInRange = false;
        }

        
    }


    private void MoveEnemy()
    {
        if(playerIsInRange == false)
        {
            
            
            transform.LookAt(player.transform);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }
    
    
}

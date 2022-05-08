using System;
using System.Collections;
using System.Collections.Generic;
using Ludiq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BattleController bController;
    private Transform target;
    private Rigidbody _rb;

    private float launchVelocity = 20f;
    private void Awake()
    {
        bController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleController>();
        _rb = this.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        target = bController.GetEnemySelected().transform;
    }

    private void FixedUpdate()
    {
        _rb.AddForce((target.position - transform.position) * launchVelocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(this);
        }
    }
}

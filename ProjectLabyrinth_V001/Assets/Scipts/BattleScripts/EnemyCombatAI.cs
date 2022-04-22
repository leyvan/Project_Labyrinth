using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCombatAI : MonoBehaviour
{
    private Canvas enemyInfo;
    private Slider enemyHealth;
    private Animator _animator;

    private float health = 1f;
    private float maxHealth = 1f;
    private float attack;

    private bool dead = false;

    private void Awake()
    {
        enemyInfo = this.transform.GetChild(1).GetComponent<Canvas>();
        enemyHealth = enemyInfo.transform.GetComponentInChildren<Slider>();

        _animator = this.transform.GetChild(2).GetComponent<Animator>();
    }

    private void Start()
    {
        enemyInfo.gameObject.SetActive(true);
    }

    public void SetDamage(float newAttack)
    {
        attack = newAttack;
    }

    public float GetAttackDmg()
    {
        return attack;
    }

    public void DoAttack()
    {
        _animator.SetTrigger("attack");
    }

    IEnumerator DoHitAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        _animator.SetTrigger("gotHit");
        SetHealthBar();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(DoHitAnimation());
        
    }

    public void SetHealth(float newHealth, float newMaxHealth)
    {
        health = newHealth;
        maxHealth = newMaxHealth;
        SetHealthBar();
    }

    private void SetHealthBar()
    {
        enemyHealth.value = health/maxHealth;
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    public Slider GetHealthBar()
    {
        return enemyHealth;
    }

    public void SetDead(bool value)
    {
        dead = value;
    }

    public bool GetDeadBool()
    {
        return dead;
    }
}

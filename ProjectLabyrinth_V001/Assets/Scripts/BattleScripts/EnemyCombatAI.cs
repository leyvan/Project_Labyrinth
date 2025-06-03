using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CombatUtil;

public class EnemyCombatAI : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathParticles;
    public EnemyType enemyType;
    private Canvas enemyInfo;
    private Slider enemyHealth;
    private Animator _animator;
    
    private bool dead = false;

    private ParticleSystem impact;

    [SerializeField] private EnemyStats enemyStats;

    private void Awake()
    {
        enemyInfo = this.transform.GetChild(0).GetComponent<Canvas>();
        enemyHealth = enemyInfo.transform.GetComponentInChildren<Slider>();

        _animator = this.transform.GetChild(1).GetComponent<Animator>();
        impact = this.transform.GetComponentInChildren<ParticleSystem>();

        enemyStats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        enemyInfo.gameObject.SetActive(true);
        enemyStats.SetEnemyStats(enemyType);
    }

    public void SetDamage(int newAttack)
    {
        enemyStats.thisEnemy.attack = newAttack;
    }

    public float GetAttackDmg()
    {
        return enemyStats.thisEnemy.attack;
    }

    public void DoAttack()
    {
        _animator.SetTrigger("attack");
    }

    IEnumerator DoHitAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        _animator.SetTrigger("gotHit");
        impact.Play();
        SetHealthBar();
    }
    
    IEnumerator DoDeathAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        _animator.SetTrigger("gotHit");
        yield return new WaitForSeconds(0.6f);
        transform.Find("Skeleton@Skin").gameObject.SetActive(false);
        enemyHealth.gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        enemyStats.thisEnemy.hpCur -= damage;
        StartCoroutine(DoHitAnimation());
    }

    public void SetHealth(float newHealth)
    {
        enemyStats.thisEnemy.hpCur = newHealth;
        SetHealthBar();
    }

    private void SetHealthBar()
    {
        var currHp = enemyStats.thisEnemy.hpCur;
        var maxHp = enemyStats.thisEnemy.hpMax;
        enemyHealth.value = currHp/maxHp;
    }

    public float GetCurrentHealth()
    {
        return enemyStats.thisEnemy.hpCur;
    }

    public void OnThisEnemyDead()
    {
        deathParticles.Play();
        StartCoroutine(DoDeathAnimation());
        dead = true;
        enemyStats.thisEnemy.hpCur = 0;
        SetHealthBar();
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

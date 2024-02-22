using System;
using UnityEngine;

public class ShadowStalkerState : MonsterState
{
    [SerializeField] private float stealthTime = 5.0f;
    [SerializeField] private float unstealthDistance = 2.0f;
    private float stealthTimer;

    protected override void Start()
    {
        base.Start();
        EnterStealthMode();
        healthSystem.OnDeath += HandleMonsterDeath;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected void EnterStealthMode()
    {
        currentState = State.Idle;
        stealthTimer = stealthTime;

        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        GetComponent<Collider2D>().enabled = false;
    }

    protected void ExitStealthMode()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GetComponent<Collider2D>().enabled = true;
    }

    protected override void IdleBehavior()
    {
        if (stealthTimer > 0)
        {
            stealthTimer -= Time.deltaTime;
        }
        else
        {
            if (Vector2.Distance(transform.position, player.transform.position) < unstealthDistance)
            {
                ExitStealthMode();
                currentState = State.Chase;
            }
        }
    }

    protected override void ChasePlayer()
    {
        animator.SetTrigger("Move");
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, statHandler.CurrentMonsterStats.speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, player.transform.position) <= statHandler.CurrentMonsterStats.attackRange)
        {
            currentState = State.Attack;
        }
    }

    protected override void AttackPlayer()
    {
        animator.SetTrigger("Attack");
        player.GetComponent<HealthSystem>().ChangeHealth(-statHandler.CurrentMonsterStats.attackDamage);
        currentState = State.Chase;
    }

    protected override void OnDeath()
    {
        ExitStealthMode();
        animator.SetTrigger("Death");

    }

    private void OnDestroy()
    {
        healthSystem.OnDeath -= HandleMonsterDeath;
    }

    private void HandleMonsterDeath()
    {
        Destroy(gameObject);
    }
}
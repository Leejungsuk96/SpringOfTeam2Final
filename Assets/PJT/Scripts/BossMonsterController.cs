using UnityEngine;
using UnityEngine.AI;

public class BossMonsterController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public CharacterStatHandler statHandler;
    public HealthSystem healthSystem;

    private float lastAttackTime;
    private float lastSpecialAttackTime;
    public float initialSpecialAttackCooldown = 10f;
    private float currentSpecialAttackCooldown;
    private HealthSystem targetHealthSystem;

    private void Start()
    {
        targetHealthSystem = target.GetComponent<HealthSystem>();
        statHandler = GetComponent<CharacterStatHandler>();
        healthSystem = GetComponent<HealthSystem>();
        currentSpecialAttackCooldown = initialSpecialAttackCooldown;
    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (distanceToTarget <= statHandler.CurrentStats.specificSO.attackRange)
        {
            if (Time.time - lastAttackTime >= statHandler.CurrentStats.attackDelay)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            ChaseTarget();
        }

        if (Time.time - lastSpecialAttackTime >= currentSpecialAttackCooldown)
        {
            PerformSpecialAttack();
            lastSpecialAttackTime = Time.time;
        }

        // ü���� 50% �̸��� ��� ��Ÿ�� ����
        if (healthSystem.CurrentHealth <= healthSystem.MaxHealth * 0.5f)
        {
            currentSpecialAttackCooldown = initialSpecialAttackCooldown / 2;
        }
    }


    private void ChaseTarget()
    {
        agent.SetDestination(target.position);
    }

    private void Attack()
    {
        if (targetHealthSystem != null)
        {
            targetHealthSystem.ChangeHealth(-statHandler.CurrentStats.attackDamage);
            Debug.Log("����!");
        }
    }

    private void PerformSpecialAttack()
    {
        JumpAndDamage(target.position);
    }

    private void JumpAndDamage(Vector3 targetPosition)
    {
        // ������ �÷��̾��� ��ġ�� ����
        // ������ �� �������� �ִ� ���� ����
        // ���� ������
    }
}

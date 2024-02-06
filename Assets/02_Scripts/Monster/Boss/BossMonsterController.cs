using UnityEngine;
using UnityEngine.AI;

public class BossMonsterController : MonoBehaviour
{
    public Transform target;
    public CharacterStatHandler statHandler;
    public HealthSystem healthSystem;

    private float lastAttackTime;
    private float lastSpecialAttackTime;
    public float initialSpecialAttackCooldown = 10f;
    protected float currentSpecialAttackCooldown;
    private HealthSystem targetHealthSystem;

    protected virtual void Start()
    {
        InitializeBoss();
    }

    protected void Update()
    {
        PerformStandardActions();
        CheckHealthAndAdjustBehavior();
    }

    private void InitializeBoss()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
            targetHealthSystem = playerObject.GetComponent<HealthSystem>();
        }

        statHandler = GetComponent<CharacterStatHandler>();
        healthSystem = GetComponent<HealthSystem>();
        currentSpecialAttackCooldown = initialSpecialAttackCooldown;
    }

    protected void PerformStandardActions()
    {
        if (ShouldAttack())
        {
            Attack();
        }

        if (ShouldPerformSpecialAttack())
        {
            PerformSpecialAttack();
        }
    }

    private bool ShouldAttack()
    {
        if (target == null)
        {
            // Ÿ���� ���� �� ���
            return false;
        }

        return Vector3.Distance(target.position, transform.position) <= statHandler.CurrentStats.specificSO.attackRange
            && Time.time - lastAttackTime >= statHandler.CurrentStats.attackDelay;
    }



    private void Attack()
    {
        // �Ϲ� ���� ����
        lastAttackTime = Time.time;
    }

    private bool ShouldPerformSpecialAttack()
    {
        return Time.time - lastSpecialAttackTime >= currentSpecialAttackCooldown;
    }

    protected virtual void PerformSpecialAttack()
    {
        lastSpecialAttackTime = Time.time;
    }

    private void CheckHealthAndAdjustBehavior()
    {
        if (healthSystem.CurrentHealth <= healthSystem.MaxHealth * 0.5f)
        {
            // ü���� 50% �̸��� ��� ����ȭ ����
        }
    }
}

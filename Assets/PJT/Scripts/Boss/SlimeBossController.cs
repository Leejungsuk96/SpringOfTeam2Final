using UnityEngine;

public class SlimeBossController : BossMonsterController
{
    private bool isEnraged = false; // ����ȭ ���� �÷���
    private float lastJumpAttackTime = 0f;
    private float jumpAttackCooldown = 5.0f; // ���� ���� ��Ÿ��
    public float maxChaseDistance = 15f; // �÷��̾� ���� �ִ� �Ÿ�
    [SerializeField] private float damageRadius = 5.0f; // �ν����� â���� ���� ������ ������ ����
    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (IsPlayerWithinChaseDistance())
        {
            PerformSpecialAttack();
        }
        else
        {
            StopChaseAndWait();
        }
    }

    protected override void PerformSpecialAttack()
    {
        if (Time.time - lastJumpAttackTime >= jumpAttackCooldown)
        {
            StartCoroutine(SpecialAttacks.JumpAndDamage(transform, target, statHandler.CurrentStats.attackDamage, 5.0f, () =>
            {
                DealDamageAtPosition(target.position, statHandler.CurrentStats.attackDamage);
            }));

            lastJumpAttackTime = Time.time;
        }

        EnrageIfNeeded();
    }

    private bool IsPlayerWithinChaseDistance()
    {
        return Vector3.Distance(transform.position, target.position) <= maxChaseDistance;
    }

    private void StopChaseAndWait()
    {
        rb.velocity = Vector2.zero;

        //animator.SetTrigger("Idle");  ���� �ִϸ��̼� �߰� �ϸ��
        Debug.Log("������ ��� ���·� ��ȯ");
    }

    private void DealDamageAtPosition(Vector3 position, float damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, damageRadius);
        foreach (var hitCollider in hitColliders)
        {
            HealthSystem healthSystem = hitCollider.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.ChangeHealth(-damage);
            }
        }
        Debug.Log("�÷��̾�� ������ " + damage);
    }

    private void EnrageIfNeeded()
    {
        if (!isEnraged && healthSystem.CurrentHealth <= healthSystem.MaxHealth * 0.5f)
        {
            isEnraged = true;
            currentSpecialAttackCooldown /= 2; // ����ȭ ȿ���� Ư�� ���� ��Ÿ�� ����
        }
    }
}

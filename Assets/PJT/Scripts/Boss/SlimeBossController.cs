using UnityEngine;

public class SlimeBossController : BossMonsterController
{
    private bool isEnraged = false; // ����ȭ ���� �÷���
    private float lastJumpAttackTime = 0f;
    private float jumpAttackCooldown = 5.0f; // ���� ���� ��Ÿ��

    private void Update()
    {
        PerformActions();
    }

    protected virtual void PerformActions()
    {
        // ��Ÿ�� üũ
        if (Time.time - lastJumpAttackTime >= jumpAttackCooldown)
        {
            // ���� ���� ����
            StartCoroutine(SpecialAttacks.JumpAndDamage(transform, target, statHandler.CurrentStats.attackDamage, 5.0f, () =>
            {
                DealDamageAtPosition(target.position, statHandler.CurrentStats.attackDamage);
            }));

            // ��Ÿ�� ����
            lastJumpAttackTime = Time.time;
        }
    }

    private void DealDamageAtPosition(Vector3 position, float damage)
    {
        float damageRadius = 5.0f; // �������� �ִ� �ݰ� ����
        Collider[] hitColliders = Physics.OverlapSphere(position, damageRadius);

        foreach (var hitCollider in hitColliders)
        {
            HealthSystem healthSystem = hitCollider.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.ChangeHealth(-damage); // ������ ����
            }
        }

        Debug.Log("�÷��̾�� ������ " + damage);
    }

    private void EnrageIfNeeded()
    {
        // ü���� 50% �̸��� ��� ����ȭ ����
        if (!isEnraged && healthSystem.CurrentHealth <= healthSystem.MaxHealth * 0.5f)
        {
            isEnraged = true;
            currentSpecialAttackCooldown /= 2; //����ȭ ȿ���� ���� ����
        }
    }
}

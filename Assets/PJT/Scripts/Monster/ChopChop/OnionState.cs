using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnionState : MonsterState
{
    private CharacterStatHandler stathandler;

    private void Start()
    {
        base.Start();
        stathandler = GetComponent<CharacterStatHandler>();
        healthSystem.OnDeath += HandleMonsterDeath;

        if (stathandler == null)
        {
            Debug.LogError("�����ڵ鷯�� �����ϴ�.");
        }
        else
        {
            stathandler.UpdateCharacterStats();
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        HandleMonsterDeath();
        Destroy(gameObject);
    }

    private void HandleMonsterDeath()
    {
        Vector3 originalPosition = transform.position;
        if (ItemManager.instance != null && ItemManager.instance.itemPool != null)
        {
            ItemManager.instance.itemPool.ItemSpawn(1713, originalPosition);
            ItemManager.instance.itemPool.ItemSpawn(1723, originalPosition);
        }
        else
        {
            Debug.LogWarning("ItemManager �Ǵ� itemPool�� null�Դϴ�. �������� ����� �� �����ϴ�.");
        }
    }

    private void OnDestroy()
    {
        if (healthSystem != null)
        {
            healthSystem.OnDeath -= HandleMonsterDeath;
        }
    }
}
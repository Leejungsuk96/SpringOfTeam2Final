using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    protected CharacterStatHandler statsHandler { get; private set; }

    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action OnAttackEvent;
    public event Action OnSetEvent;
    public event Action OnInteractEvent;
    public event Action OnEquipEvent;

    private float timeSinceLastAttack = float.MaxValue;
    private float timeSinceLastEquip = float.MaxValue;
    public bool IsAttacking { get; set; }
    public bool IsSetting { get; set; }

    public bool IsInteracting { get; set; }
    public bool IsEquiping {  get; set; }
    public bool CanControllCharacter { get; set; }
    private Vector3 spawnPoint = Vector3.zero;

    protected virtual void Awake()
    {
        statsHandler = GetComponent<CharacterStatHandler>();
        CanControllCharacter = true;
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.OnDeath += OnDeath;
        }
    }
    protected virtual void Update()
    {
        AttackDelay();
        CanInteract();
        CanSet();       
    }

    private void AttackDelay() // ���� ������ ȿ�� 
    {
        if (statsHandler.CurrentStats.specificSO == null)
            return;

        if(timeSinceLastAttack <= statsHandler.CurrentStats.attackDelay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
        if(IsAttacking && timeSinceLastAttack > statsHandler.CurrentStats.attackDelay)
        {
            timeSinceLastAttack = 0;
            CallAttackEvent();
        }
    }
   

    private void CanInteract() // ���⼭ ��ȣ�ۿ� ���� ������Ʈ üũ�ϰ� �Ÿ� ��������
    {
        if (IsInteracting)
        {
            Debug.Log(IsInteracting);
            CallInteractEvent();
        }
    }

    private void CanSet() // ���⼭ ��ġ ������ ������Ʈ ���� �� ó�� �ؾ���
    {
        if (IsSetting)
        {
            CallSetEvent();
        }
    }

    private void OnDeath()
    {
        transform.position = spawnPoint;
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            healthSystem.ChangeHealth(healthSystem.MaxHealth);
            Debug.Log("ȸ��");
        }
    }

    public void CallMoveEvent(Vector2 value)
    {
        OnMoveEvent?.Invoke(value);
    }

    public void CallLookEvent(Vector2 value)
    {
        OnLookEvent?.Invoke(value);
    }

    public void CallAttackEvent()
    {
        OnAttackEvent?.Invoke();
    }

    public void CallSetEvent()
    {
        OnSetEvent?.Invoke();
    }

    public void CallInteractEvent()
    {
        OnInteractEvent?.Invoke();
    }

    public void CallEquipEvent()
    {
        OnEquipEvent?.Invoke();
    }
}

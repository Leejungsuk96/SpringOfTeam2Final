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
    private float timeSinceLastSet = float.MaxValue;
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

    private void AttackDelay() // 공격 딜레이 효과 
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
   

    private void CanInteract() // 여기서 상호작용 가능 오브젝트 체크하고 거리 설정까지
    {
        if (IsInteracting)
        {
            CallInteractEvent();
        }
    }

    private void CanSet() // 여기서 설치 가능한 오브젝트 조건 등 처리 해야함
    {
        if (timeSinceLastSet <= 0.2f)
        {
            timeSinceLastSet += Time.deltaTime;            
        }
        if(IsSetting && timeSinceLastSet > 0.2f)
        {
            timeSinceLastSet = 0;
            CallSetEvent();
        }
    }

    private void OnDeath()
    {
        transform.position = spawnPoint;
        StartCoroutine(RecoverHealthAfterDelay(0.6f));
    }

    IEnumerator RecoverHealthAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        if (healthSystem != null)
        {
            float healthToRecover = healthSystem.MaxHealth - healthSystem.CurrentHealth;
            healthSystem.ChangeHealth(healthToRecover);
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

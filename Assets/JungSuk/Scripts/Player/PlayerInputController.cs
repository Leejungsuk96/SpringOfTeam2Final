using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : CharacterController
{
    private Camera _camera; // OnLook���� ���콺 ��ġ ���͸� �˼� �ֵ��� �����´�.


    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    public void OnMove(InputValue value)
    {
        Vector2 InputMoveValue;
        InputMoveValue = value.Get<Vector2>().normalized;
        CallMoveEvent(InputMoveValue);
       
    }
    
    public void OnLook(InputValue value)
    {
        Vector2 InputLookValue = value.Get<Vector2>();
        Vector2 worldPos = _camera.ScreenToWorldPoint(InputLookValue);
        InputLookValue = (worldPos - (Vector2)transform.position).normalized;

        if(InputLookValue.magnitude >= 0.0f)
        {
            CallLookEvent(InputLookValue);
        }
        
    }

    public void OnAttack(InputValue value)
    {
        IsAttacking = value.isPressed;
    }

    public void OnSet(InputValue value)
    {
        IsSetting = value.isPressed;
    }
}

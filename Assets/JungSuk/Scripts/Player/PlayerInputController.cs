using Microsoft.Win32.SafeHandles;
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

        if (CanControllCharacter == false)
            InputMoveValue = Vector2.zero;

        else
        {
            InputMoveValue = value.Get<Vector2>().normalized;
            CallMoveEvent(InputMoveValue);
        }
    }
    
    public void OnLook(InputValue value)
    {
        Vector2 InputLookValue = value.Get<Vector2>();
        Vector2 worldPos = _camera.ScreenToWorldPoint(InputLookValue);
        InputLookValue = (worldPos - (Vector2)transform.position).normalized;

        if (CanControllCharacter == false)
        {
            return;
        }
        else
        {
            if (InputLookValue.magnitude >= 0.0f)
            {
                CallLookEvent(InputLookValue);
            }
        }
    }

    public void OnAttack(InputValue value)
    {
        if (CanControllCharacter == false)
            return;
        else
        IsAttacking = value.isPressed;
    }

    public void OnSet(InputValue value)
    {
        if (CanControllCharacter == false)
            return;
        else
        IsSetting = value.isPressed;
    }

    public void OnInteract(InputValue value)
    {        
        IsInteracting = value.isPressed;
    }

    public void OnEquip(InputValue value)
    {       
        if (CanControllCharacter == false)
            return;
        else
        {
            CallEquipEvent();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveValue = Vector2.zero; // �ŰԺ��� �� �־��༭ �� Ŭ���� ������ ����� ���Ͱ�
    private Rigidbody2D rigidbody;
    private CharacterStatHandler statsHandler;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();       
        rigidbody = GetComponent<Rigidbody2D>();
        statsHandler = GetComponent<CharacterStatHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        controller.OnMoveEvent += Move;
    }

    private void Move(Vector2 value)
    {        
            moveValue = value;
    }

    private void ApplyMovement(Vector2 value)
    {
        value = value * statsHandler.CurrentStats.speed; // ���߿� ĳ���� ���ǵ� ������ ��ü ����(�Ϸ�)
        rigidbody.velocity = value;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (controller.CanControllCharacter)
        {
            ApplyMovement(moveValue);
        }
        else
            ApplyMovement(Vector2.zero);
    }
}

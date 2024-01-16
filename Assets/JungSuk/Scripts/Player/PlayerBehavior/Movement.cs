using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController _controller;
    private Vector2 _moveValue = Vector2.zero; // �ŰԺ��� �� �־��༭ �� Ŭ���� ������ ����� ���Ͱ�
    private Rigidbody2D _rigidbody;
    private CharacterStatHandler statsHandler;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();       
        _rigidbody = GetComponent<Rigidbody2D>();
        statsHandler = GetComponent<CharacterStatHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _controller.OnMoveEvent += Move;
    }

    private void Move(Vector2 value)
    {
        _moveValue = value;
    }

    private void ApplyMovement(Vector2 value)
    {
        value = value * statsHandler.CurrentStats.baseStatsSO.speed; // ���߿� ĳ���� ���ǵ� ������ ��ü ����
        _rigidbody.velocity = value;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        ApplyMovement(_moveValue);
    }
}

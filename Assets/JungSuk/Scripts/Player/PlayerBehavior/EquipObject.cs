using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipObject : MonoBehaviour
{
    private CharacterController controller;
    public Image[] quitSlots;
    public SpriteRenderer heldItem;
    public Transform SpawnTrans;
    private CharacterStatHandler statHandler;

    private Inventory inventory;

    private void Awake()
    {        
        controller = GetComponent<CharacterController>();
        statHandler = GetComponent<CharacterStatHandler>();
        inventory = GetComponent<Inventory>();
    }
    
    void Start()
    {
        controller.OnEquipEvent += SelectItemInQuikSlot;
    }

    private void SelectItemInQuikSlot()
    {
        for (int i = 0; i < 8; i++)
        {
            inventory.invenSlot[i].QuickSlotItemChoose(false);
            inventory.slots[i].isChoose = false;
        }

        for (int i = 1; i <= 8; i++)
        {
            KeyCode key = KeyCode.Alpha0 + i;            
            if (Input.GetKeyDown(key))
            {
                if (inventory.slots[i - 1].isChoose == false) // isChoose�� �ι� ������ �ȿ� �ִ� �޼���� ����ȴ�.
                {
                    EquipItem(i - 1); // ������ ���
                    break;
                }
                // ���� else�� ���ָ� ���� Ű�� �ι� ������ �� ������.
            }
            /*else
            {
                inventory.slots[i - 1].isChoose = false;
                inventory.invenSlot[i - 1].QuickSlotItemChoose(false);
            }*/
        }
    }

    private void EquipItem(int slotIndex)
    {
        heldItem.sprite = quitSlots[slotIndex].sprite;
        inventory.slots[slotIndex].isChoose = true;
        inventory.invenSlot[slotIndex].QuickSlotItemChoose(true);
        
    }

    private void UnEquipItem(int slotIndex)// todo ������ �ִ� ������ ������ ����ֱ�.
    {/*
        heldItem.sprite = null;
        inventory.invenSlot[slotIndex].QuickSlotItemChoose(false);*/
    }


    /*private void ChangeStatByItem(int Index)
    {
        if (UIManager.Instance.AllItemList[Index - 1].ItemType == "WeaponRange")
        {
            if (IsEquipedItem == true)
            {
                statHandler.CurrentStats.attackDamage += float.Parse(UIManager.Instance.AllItemList[Index - 1].AttackDamage);
                Debug.Log(statHandler.CurrentStats.attackDamage);
            }
            else
                return;
        }
        else
            return;        
    }*/
}
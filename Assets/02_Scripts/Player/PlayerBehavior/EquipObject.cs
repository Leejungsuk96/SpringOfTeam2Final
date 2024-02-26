using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipObject : MonoBehaviour, IEquipedItem
{
    private CharacterController controller;
    public Image[] quitSlots;
    public SpriteRenderer heldItem;
    public Transform SpawnTrans;
    private CharacterStatHandler statHandler;

    private Inventory inventory;
    private int selectedIndexNum = 20;

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
            if (i == selectedIndexNum && inventory.slots[selectedIndexNum].isChoose == true)
            {
                UnEquipItemForChangeStats(selectedIndexNum);
            }
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
                    if (inventory.slots[i-1].item != null)
                    {
                        EquipItemForChangeStats(i - 1);
                        selectedIndexNum = i - 1;
                    }
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

    public void EquipItemForChangeStats(int itemIndex)
    {
        if (inventory.slots[itemIndex] == null)
        {
            return;            
        }
        else
        {            
             if (inventory.slots[itemIndex].item.ItemType == 10 || inventory.slots[itemIndex].item.ItemType == 11)
             {
                statHandler.CurrentStats.attackDamage += inventory.slots[itemIndex].item.AttackDamage;
             }

             else if (inventory.slots[itemIndex].item.ItemType == 12)
             {
                statHandler.CurrentStats.miningAttack += inventory.slots[itemIndex].item.AttackDamage;
             }
             else
             {
                return;
             }
            UIManager.Instance.UpdatePlayerStatTxt();
        }
    }

    public void UnEquipItemForChangeStats(int itemIndex)
    {
        if (inventory.slots[itemIndex] == null)
        {
            return;
        }
        else
        {            
            if (inventory.slots[itemIndex].item.ItemType == 10)
            {
                statHandler.CurrentStats.attackDamage -= inventory.slots[itemIndex].item.AttackDamage;
            }
            else if (inventory.slots[itemIndex].item.ItemType == 12)
            {
                statHandler.CurrentStats.miningAttack -= inventory.slots[itemIndex].item.AttackDamage;
            }
            else
            {
                return;
            }                     
        }
        UIManager.Instance.UpdatePlayerStatTxt();
    }

    /*private void UnEquipItem(int slotIndex)// todo ������ �ִ� ������ ������ ����ֱ�.
    {*//*
        heldItem.sprite = null;
        inventory.invenSlot[slotIndex].QuickSlotItemChoose(false);*//*
    }*/


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
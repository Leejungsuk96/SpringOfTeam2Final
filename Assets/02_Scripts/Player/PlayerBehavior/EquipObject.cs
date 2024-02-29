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

    private Item previousEquipItemData;
    public Item usedPreviousEquipItemData { get { return previousEquipItemData; } private set { previousEquipItemData = value; } }
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
            // UI���ֱ�
            inventory.invenSlot[i].QuickSlotItemChoose(false);
            inventory.slots[i].isChoose = false;
        }
        if (previousEquipItemData != null)
        {
            UnEquipItemForChangeStats(previousEquipItemData.ItemCode);
        }

        for (int i = 1; i <= 8; i++)
        {
            KeyCode key = KeyCode.Alpha0 + i;            
            if (Input.GetKeyDown(key))
            {
                inventory.slots[i - 1].isChoose = true;

                if (inventory.slots[i - 1].isChoose) 
                {
                    // �÷��̾ �ƹ��͵� ������������ ���¿��� �������� ���°� ó�� �������� ��
                    if (previousEquipItemData == null)
                    {
                        previousEquipItemData = inventory.slots[i - 1].item;
                    }
                    // �����Կ��� ������ �������� �Ű��� �ڸ��� �ٽ� ������ ��.
                    if (inventory.slots[i - 1].item == null)
                    {
                        previousEquipItemData = null;
                    }
                    // ������ ����� �������̶� ���ٸ�
                    if (previousEquipItemData == inventory.slots[i - 1].item)
                    {
                        EquipItem(i - 1);
                        inventory.invenSlot[i - 1].QuickSlotItemChoose(true);
                        inventory.slots[i - 1].isChoose = true;
                        EquipItemForChangeStats(i - 1);
                        break;
                    }
                    // �ٸ��ٸ�
                    else
                    {
                        EquipItem(i - 1); // ������ ���

                        if (inventory.slots[i-1].item != null)
                        {
                            // ���� ������ �������̸��� ������ �÷��ָ� �ȉ´�.
                            if (!inventory.slots[i - 1].item.IsEquip)
                            {
                                EquipItemForChangeStats(i - 1);
                            }
                            previousEquipItemData = inventory.slots[i - 1].item;
                        }
                        break;

                    }
                }
            }
        }
    }

    private void EquipItem(int slotIndex)
    {
        heldItem.sprite = quitSlots[slotIndex].sprite;
        inventory.invenSlot[slotIndex].QuickSlotItemChoose(true);
    }

    public void EquipItemForChangeStats(int itemIndex)
    {
        if (inventory.slots[itemIndex].item == null)
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

    public void UnEquipItemForChangeStats(int itemCode)
    {
        if (previousEquipItemData.ItemType == 10)
        {
            statHandler.CurrentStats.attackDamage -= previousEquipItemData.AttackDamage;
        }
        else if (previousEquipItemData.ItemType == 12)
        {
            statHandler.CurrentStats.miningAttack -= previousEquipItemData.AttackDamage;
        }
        else
        {
            return;
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
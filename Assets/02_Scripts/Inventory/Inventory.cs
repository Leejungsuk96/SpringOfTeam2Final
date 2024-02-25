using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<SlotData> slots = new();// SlotData�� ����Ʈ�� ���������.
    public SlotNum[] invenSlot;

    public void Awake()
    {
        for(int i = 0; i < invenSlot.Length; i++)
        {
            SlotData slot = new()// ��ü �ʱ�ȭ �ܼ�ȭ
            {
                isEmpty = true,
                isChoose = false,
                item = null,
                stack = 0
            };
            slots.Add(slot);
        }
    }

    // �κ��丮 �������� ���ð˻�.
    public bool CheckStackAmount(int itemCode, int requiredStack)
    {
        // �κ��丮 ��ĵ
        for (int i = 0; i < invenSlot.Length - 3; i++)
        {
            if (!slots[i].isEmpty && slots[i].item.ItemCode == itemCode)
            {
                if (slots[i].stack >= requiredStack)
                {
                    return true;
                }
                else return false;
            }
        }
        return false;
    }

    // �κ��丮 ���� �˻�.
    public void StackUpdate(int indexOfInventory)
    {
        if (slots[indexOfInventory].stack == 0)
        {
            slots[indexOfInventory].item = null;
            slots[indexOfInventory].isEmpty = true;
            invenSlot[indexOfInventory].ChangeInventoryImage(0);
            invenSlot[indexOfInventory].OnOffImage(0);
        }
        else if (slots[indexOfInventory].stack > 0)
        {
            slots[indexOfInventory].isEmpty = false;
            invenSlot[indexOfInventory].ChangeInventoryImage(slots[indexOfInventory].item.ItemCode);
            invenSlot[indexOfInventory].OnOffImage(1f);
        }
        invenSlot[indexOfInventory].ItemStackUIRefresh(slots[indexOfInventory].stack);
    }

    // �κ��丮���� ������ �������ֱ�.
    public void RemoveItemFromInventory(int itemCode, int stackNeed)
    {
        // �κ��丮 ��ĵ
        for (int i = 0; i < invenSlot.Length - 3; i++)
        {
            if (slots[i].item != null && slots[i].item.ItemCode == itemCode)
            {
                // ������  ����� Stack�� ����� �ͺ��� ���� �� �����̵ɰŶ� ������ �� ������ ��� if������ �ȳ־��൵��.
                slots[i].stack -= stackNeed;
                StackUpdate(i);
                break;
            }
        }
    }

    public void GiveItemToEmptyInv(Item itemData, int stack)
    {
        // ������ �ӽú����ҿ� �����ֱ�.
        UIManager.Instance.giveTemporaryItemData = itemData;
        UIManager.Instance.giveTemporaryItemStack = stack;

        for (int i = 0; i < invenSlot.Length - 3; i++)
        {
            if (slots[i].isEmpty)
            {
                slots[i].item = UIManager.Instance.giveTemporaryItemData;
                slots[i].stack = UIManager.Instance.giveTemporaryItemStack;
                StackUpdate(i);


                UIManager.Instance.giveTemporaryItemData = null;
                UIManager.Instance.giveTemporaryItemStack = 0;
                break;
            }
            else
            {
                if (slots[i].item.ItemCode == itemData.ItemCode)
                {
                    slots[i].stack += stack;
                    StackUpdate(i);
                    break;
                }
                continue;
            }
        }
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    public int itemCode;
    public Item item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        item = ItemManager.instance.SetItemData(itemCode); // itemIndex�� �������� �������� �����ش�.
        if (collision.CompareTag("Player"))
        {
            Inventory inven = collision.GetComponent<Inventory>();
            for (int i = 0; i < inven.invenSlot.Length - 3; i++)
            {
                // �κ��丮�� ���� ������ �ڵ��� �������� �ִٸ�, stack�� �÷��ְ� ����.
                if (!inven.slots[i].isEmpty && inven.slots[i].item.ItemCode == itemCode)
                {
                    inven.slots[i].stack++;
                    inven.invenSlot[i].ItemStackUIRefresh(inven.slots[i].stack);
                    inven.StackUpdate(i);
                    gameObject.SetActive(false);
                    return;
                }
            }
            for (int i = 0; i < inven.invenSlot.Length - 3; i++)
            {
                if (inven.slots[i].isEmpty)
                {
                    inven.invenSlot[i].ChangeInventoryImage(itemCode);
                    inven.invenSlot[i].OnOffImage(1f);
                    inven.slots[i].isEmpty = false;
                    inven.slots[i].item = item; // ������ �������� �����͸� �־��ش�.
                    inven.slots[i].stack = 1;
                    inven.invenSlot[i].ItemStackUIRefresh(inven.slots[i].stack);
                    inven.StackUpdate(i);
                    if (inven.slots[i].isChoose)
                    {
                        // �� ������ �������� ���� �տ� �̹��� ��Ÿ���� ����.
                        collision.GetComponent<EquipObject>().heldItem.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                    }
                    gameObject.SetActive(false);
                    return;
                }
            }
        }
    }
}

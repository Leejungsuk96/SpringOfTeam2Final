using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    public int itemIndex;
    public Item item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetItemInfo(itemIndex); // itemIndex�� �������� �������� �����ش�.
        if (collision.CompareTag("Player"))
        {
            Inventory inven = collision.GetComponent<Inventory>();
            for (int i = 0; i < inven.invenSlot.Length; i++)
            {
                // �κ��丮�� ���� ������ �ڵ��� �������� �ִٸ�, stack�� �÷��ְ� ����.
                if (!inven.slots[i].isEmpty && inven.slots[i].item.ItemCode == itemIndex)
                {
                    inven.slots[i].stack++;
                    inven.invenSlot[i].ItemStackUIRefresh(inven.slots[i].stack);
                    gameObject.SetActive(false);
                    break;
                }
                else if (inven.slots[i].isEmpty)
                {
                    inven.invenSlot[i].ChangeInventoryImage(gameObject.GetComponent<SpriteRenderer>().sprite);
                    inven.invenSlot[i].OnOffImage(true);
                    inven.slots[i].isEmpty = false;
                    inven.slots[i].item = item; // ������ �������� �����͸� �־��ش�.
                    inven.slots[i].stack = 1;
                    inven.invenSlot[i].ItemStackUIRefresh(inven.slots[i].stack);

                    if (inven.slots[i].isChoose)
                    {
                        // �� ������ �������� ���� �̹��� ��Ÿ���� ����.
                        collision.GetComponent<EquipObject>().heldItem.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                        // �����Կ��� ������ �������� �� �÷��̾����� ������ �־��ִ� �۾��� �����ϸ� ���⿡���� �ٷ� �÷��̾�� ������ ������ �����ֱ�.
                        // �۾� ���� ���۾���.
                    }
                    gameObject.SetActive(false);
                    break;
                }
            }
        }
    }

    private void SetItemInfo(int Index)
    {
        item = ItemManager.instacne.items[Index];
    }
}

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
                if (inven.slots[i].isEmpty)
                {
                    inven.invenSlot[i].ChangeInventoryImage(gameObject.GetComponent<SpriteRenderer>());
                    inven.invenSlot[i].OnOffImage(true);
                    inven.slots[i].isEmpty = false;
                    inven.slots[i].item = item; // ������ �������� �����͸� �־��ش�.
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

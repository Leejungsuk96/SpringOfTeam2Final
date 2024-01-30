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
        SetItemInfo(itemIndex); // itemIndex로 아이템이 무엇인지 정해준다.
        if (collision.CompareTag("Player"))
        {
            Inventory inven = collision.GetComponent<Inventory>();
            for (int i = 0; i < inven.invenSlot.Length; i++)
            {
                // 인벤토리에 같은 아이템 코드의 아이템이 있다면, stack을 올려주고 꺼짐.
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
                    inven.slots[i].item = item; // 정해준 아이템의 데이터를 넣어준다.
                    inven.slots[i].stack = 1;
                    inven.invenSlot[i].ItemStackUIRefresh(inven.slots[i].stack);

                    if (inven.slots[i].isChoose)
                    {
                        // 빈 곳으로 아이템이 들어가면 이미지 나타나게 해줌.
                        collision.GetComponent<EquipObject>().heldItem.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                        // 퀵슬롯에서 아이템 선택했을 때 플레이어한테 데이터 넣어주는 작업을 시작하면 여기에서는 바로 플레이어에게 아이템 데이터 더해주기.
                        // 작업 아직 시작안함.
                    }
                    gameObject.SetActive(false);
                    break;
                }
            }
        }
    }

    private void SetItemInfo(int Index)
    {
        item = ItemManager.instance.items[Index];
    }
}

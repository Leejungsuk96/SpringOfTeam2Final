using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodResult : MonoBehaviour
{
    private CanvasGroup itemImg;

    private void Awake()
    {
        itemImg = GetComponent<CanvasGroup>();
    }

    public void ClickButtonOnFood()
    {
        // ������ ����
        MakeFood();

        // �κ��丮�� ������ ����. ������ �ӽú����ҿ� �����ֱ�.
        UIManager.Instance.giveTemporaryItemData = UIManager.Instance.playerInventoryData.slots[28].item;
        UIManager.Instance.giveTemporaryItemStack = 1;

        for (int i = 0; i < UIManager.Instance.playerInventoryData.invenSlot.Length - 3; i++)
        {
            // ����ְų� ������ �ڵ尡 ���ٸ�
            if (UIManager.Instance.playerInventoryData.slots[i].isEmpty || UIManager.Instance.playerInventoryData.slots[i].item.ItemCode == UIManager.Instance.playerInventoryData.slots[28].item.ItemCode)
            {
                UIManager.Instance.playerInventoryData.slots[i].item = UIManager.Instance.giveTemporaryItemData;
                UIManager.Instance.playerInventoryData.slots[i].stack += UIManager.Instance.giveTemporaryItemStack;
                UIManager.Instance.StackUpdate(i);


                UIManager.Instance.giveTemporaryItemData = null;
                UIManager.Instance.giveTemporaryItemStack = 0;
                break;
                // �޼��尡 ����ǰ������ �ƴϴϱ� break;
            }
        }

        if ((UIManager.Instance.playerInventoryData.slots[26].stack == 0) || (UIManager.Instance.playerInventoryData.slots[27].stack == 0))
        {
            itemImg.blocksRaycasts = false;
            UIManager.Instance.playerInventoryData.slots[28].stack = 0;
            UIManager.Instance.StackUpdate(28);
        }
        // ���� ������ ���Ҵٸ�
        else
        {
            itemImg.blocksRaycasts = true;
        }
    }


    /*public void OnBeginDrag(PointerEventData eventData)
    {


        // �ȿ� �����͵��� ����ִٸ�
        if ((firstItem == 8) && (secondItem == 8))
        {
            // �ش� �������� Stack�� 0���� ������
            if ((UIManager.Instance.playerInventoryData.slots[26].stack > 0) && (UIManager.Instance.playerInventoryData.slots[27].stack > 0))
            {
                Debug.Log("���տϷ�");
                FoodResultStack(true);
                // �ӽ�����ҷ� Item������ ����
                UIManager.Instance.giveTemporaryItemData = UIManager.Instance.playerInventoryData.slots[inventoryIndex].item;
                // �ӽ������ ���� +1
                UIManager.Instance.giveTemporaryItemStack++;
                // ������� ������ 0�̸�
                if ((UIManager.Instance.playerInventoryData.slots[26].stack == 0) || (UIManager.Instance.playerInventoryData.slots[27].stack == 0))
                {
                    UIManager.Instance.playerInventoryData.slots[inventoryIndex].item = null;
                    UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack = 0;
                    UIManager.Instance.StackUpdate(inventoryIndex);
                }
                // ���� ������ ���Ҵٸ�
                else
                {
                    UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack--;
                    UIManager.Instance.StackUpdate(inventoryIndex);
                }
            }
        }
    }*/

    private void MakeFood()
    {
        UIManager.Instance.playerInventoryData.slots[26].stack--;
        UIManager.Instance.playerInventoryData.slots[27].stack--;

        UIManager.Instance.StackUpdate(26);
        UIManager.Instance.StackUpdate(27);
    }
}

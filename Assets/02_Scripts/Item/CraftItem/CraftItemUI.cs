using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftItemUI : MonoBehaviour
{
    // CanvasGroup�� BlockRaycats�� Ȱ���Ͽ� Drag & Drop���� ���������� BlockRaycats�� ���ָ� ������ �� �ְ� ���ش�.
    [SerializeField] private CanvasGroup[] craftItem;
    private Dictionary<int, CanvasGroup> craftItemOrder;

    internal List<int> stuffGather;
    internal int inventoryLength;

    private void Awake()
    {
        inventoryLength = UIManager.Instance.playerInventoryData.invenSlot.Length - 3;
        // CraftItemDrag.cs���� �ν�����â���� ������ ItemCode�� Ű���� �����ϰ� �ش� �ε����� CanvasGroup�� �־��ش�.
        for (int i = 0; i < craftItem.Length; i++)
        {
            int craftItemDragItemCode = craftItem[i].GetComponent<CraftItemDrag>().itemCode;
            craftItemOrder.Add(craftItemDragItemCode, craftItem[i]);
        }
    }

    private void OnEnable()
    {
        // �κ��丮 �ѹ� ��ĵ�ϱ�
        ReFreshCraftingUI();
    }

    public void ReFreshCraftingUI()
    {
        // ���� �� stuffGather �ȿ� ����ֱ�
        stuffGather = new List<int>();

        for (int i = 0; i < inventoryLength; i++)
        {
            if (!UIManager.Instance.playerInventoryData.slots[i].isEmpty)
            {
                stuffGather.Add(UIManager.Instance.playerInventoryData.slots[i].item.ItemCode);
                Debug.Log("��ĵ�Ϸ�");
            }
            else continue;
        }

        for (int j = 0; j < craftItem.Length; j++)
        {
            craftItem[j].alpha = 0.2f;
            craftItem[j].blocksRaycasts = false;
        }
        // ���ǿ� ������ ���۴뿡 �ִ� ������ ���ֱ�.
        Debug.Log("�ʱ�ȭ �Ϸ�");
        CraftingRecipe();
    }


    // ���� �˻�.
    public bool CheckStackAmount(int itemCode, int requiredStack)
    {
        // �κ��丮 ��ĵ
        for (int i = 0; i < inventoryLength; i++)
        {
            if (!UIManager.Instance.playerInventoryData.slots[i].isEmpty && UIManager.Instance.playerInventoryData.slots[i].item.ItemCode == itemCode)
            {
                if (UIManager.Instance.playerInventoryData.slots[i].stack >= requiredStack)
                {
                    return true;
                }
                else return false;
            }
        }
        return false;
    }
    public void CraftingRecipe()
    {
        Debug.Log("������ ����");
        if (stuffGather.Contains(3011) && stuffGather.Contains(3101))
        {
            // ���� �ʿ䵵 ���� ���� ��, ������ Ȱ��ȭ
            if (CheckStackAmount(3011, 1) && CheckStackAmount(3101, 2))
            {
                Debug.Log("�� Ȱ��ȭ");
                SetCraftItemImage(1001);
            }
        }
        if (stuffGather.Contains(3011) && stuffGather.Contains(3101) && stuffGather.Contains(3001))
        {
            if (CheckStackAmount(3011, 1) && CheckStackAmount(3101, 3) && CheckStackAmount(3001, 1))
            {
                Debug.Log("��� Ȱ��ȭ");
                SetCraftItemImage(1301);
            }
        }
        if (stuffGather.Contains(3101))
        {
            if (CheckStackAmount(3101, 4))
            {
                Debug.Log("���� ���� Ȱ��ȭ");
                SetCraftItemImage(1401);
            }
            else if (CheckStackAmount(3101, 5))
            {
                Debug.Log("���� ���� Ȱ��ȭ");
                SetCraftItemImage(1501);
            }
            else if (CheckStackAmount(3101, 3))
            {
                Debug.Log("���� �Ź� Ȱ��ȭ");
                SetCraftItemImage(1601);
            }
        }
        if (stuffGather.Contains(1001) && stuffGather.Contains(3102))
        {
            if (CheckStackAmount(1001, 1) && CheckStackAmount(3102, 4))
            {
                Debug.Log("ö�� Ȱ��ȭ");
                SetCraftItemImage(1002);
            }
        }
        if (stuffGather.Contains(1301) && stuffGather.Contains(3102))
        {
            if (CheckStackAmount(1301, 1) && CheckStackAmount(3102, 5))
            {
                Debug.Log("ö��� Ȱ��ȭ");
                SetCraftItemImage(1302);
            }
        }
        if (stuffGather.Contains(3102))
        {
            if (CheckStackAmount(3102, 4))
            {
                Debug.Log("ö ���� Ȱ��ȭ");
                SetCraftItemImage(1402);
            }
            else if (CheckStackAmount(3102, 5))
            {
                Debug.Log("ö ���� Ȱ��ȭ");
                SetCraftItemImage(1502);
            }
            else if (CheckStackAmount(3102, 3))
            {
                Debug.Log("ö �Ź� Ȱ��ȭ");
                SetCraftItemImage(1602);
            }
        }
    }


    private void SetCraftItemImage(int itemCode)
    {
        if (craftItemOrder.ContainsKey(itemCode))
        {
            craftItemOrder[itemCode].alpha = 1f;
            craftItemOrder[itemCode].blocksRaycasts = true;
        }
        else Debug.Log("�������´� ����");
    }
}

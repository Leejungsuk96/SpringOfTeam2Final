using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftItemUI : MonoBehaviour
{
    // CanvasGroup�� BlockRaycats�� Ȱ���Ͽ� Drag & Drop���� ���������� BlockRaycats�� ���ָ� ������ �� �ְ� ���ش�.
    [SerializeField] private CanvasGroup[] craftItem;

    internal List<int> stuffGather;
    internal int inventoryLength;

    private void Awake()
    {
        inventoryLength = UIManager.Instance.playerInventoryData.invenSlot.Length - 3;
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

    public void CraftingRecipe()
    {
        Debug.Log("������ ����");
        if (stuffGather.Contains(3011) && stuffGather.Contains(3101))
        {
            Debug.Log("������ ����.");
            // ���� �ʿ䵵 ���� ���� ��, ������ Ȱ��ȭ
            if (CheckStackAmount(3011, 1) && CheckStackAmount(3101, 1))
            {
                Debug.Log("�� Ȱ��ȭ");
                craftItem[0].alpha = 1f;
                craftItem[0].blocksRaycasts = true;
            }
        }
        if (stuffGather.Contains(3011) && stuffGather.Contains(3101) && stuffGather.Contains(3001))
        {
            if (CheckStackAmount(3011, 1) && CheckStackAmount(3101, 1) && CheckStackAmount(3001, 1))
            {
                Debug.Log("��� Ȱ��ȭ");
                craftItem[1].alpha = 1f;
                craftItem[1].blocksRaycasts = true;
            }
        }
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
}

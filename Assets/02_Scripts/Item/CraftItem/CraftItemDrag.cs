using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftItemDrag : MonoBehaviour
{
    // �ν����� â���� ������ �ڵ� ������ߴ�.
    [SerializeField] private int itemCode;
    // �巡�� ���� �� �갡 ���� ���������� �˰� �����͸� ����������ؼ�, ItemŬ������ �޾ƿ�.
    private Item storeItemData;

    private CanvasGroup itemImg;

    private CraftItemUI craftItemUI;

    private void Awake()
    {
        craftItemUI = GetComponentInParent<CraftItemUI>();

        // ������ �������� ����
        storeItemData = ItemManager.instance.SetItemData(itemCode);

        itemImg = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        gameObject.GetComponent<Image>().sprite = ItemManager.instance.GetSpriteByItemCode(itemCode);
    }

    public void ClickButtonOnStore()
    {
        // ������ ����
        CreateFromStore();

        // ������ �ӽú����ҿ� �����ֱ�.
        UIManager.Instance.giveTemporaryItemData = storeItemData;
        UIManager.Instance.giveTemporaryItemStack = 1;

        for (int i = 0; i < UIManager.Instance.playerInventoryData.invenSlot.Length - 3; i++)
        {
            if (UIManager.Instance.playerInventoryData.slots[i].isEmpty)
            {
                UIManager.Instance.playerInventoryData.slots[i].item = UIManager.Instance.giveTemporaryItemData;
                UIManager.Instance.playerInventoryData.slots[i].stack = UIManager.Instance.giveTemporaryItemStack;
                UIManager.Instance.StackUpdate(i);


                UIManager.Instance.giveTemporaryItemData = null;
                UIManager.Instance.giveTemporaryItemStack = 0;
                break;
                // �޼��尡 ����ǰ������ �ƴϴϱ� break;
            }
        }

        itemImg.blocksRaycasts = false;

        // �ٽ� ��ĵ�ϱ�.
        craftItemUI.ReFreshCraftingUI();
    }

    


    public void RemoveItemFromInventory(int itemCode, int stackNeed)
    {
        // �κ��丮 ��ĵ
        for (int i = 0; i < craftItemUI.inventoryLength; i++)
        {
            if (UIManager.Instance.playerInventoryData.slots[i].item != null && UIManager.Instance.playerInventoryData.slots[i].item.ItemCode == itemCode)
            {
                // ������  ����� Stack�� ����� �ͺ��� ���� �� �����̵ɰŶ� ������ �� ������ ��� if������ �ȳ־��൵��.
                UIManager.Instance.playerInventoryData.slots[i].stack -= stackNeed;
                if (UIManager.Instance.playerInventoryData.slots[i].stack <= 0)
                {
                    // ������ �����ְ� �̹��� �����ֱ�.
                    UIManager.Instance.StackUpdate(i);
                    // stuffGather�� �� ItemCode�����ֱ�
                    craftItemUI.stuffGather.Remove(itemCode);
                }
                break;
            }
        }
    }

    public void CreateFromStore()
    {
        // ���⿡ ���ս� �� �����ָ��.
        switch (itemCode)
        {
            case 1001:
                RemoveItemFromInventory(3011, 1);
                RemoveItemFromInventory(3101, 1);
                break;
            case 1301:
                RemoveItemFromInventory(3011, 1);
                RemoveItemFromInventory(3101, 1);
                RemoveItemFromInventory(3001, 1);
                break;
        }
    }
}

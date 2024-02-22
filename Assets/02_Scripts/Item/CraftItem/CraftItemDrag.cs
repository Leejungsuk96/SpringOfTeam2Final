using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftItemDrag : MonoBehaviour
{
    // �ν����� â���� ������ �ڵ� ������ߴ�.
    [SerializeField] internal int itemCode;
    // �巡�� ���� �� �갡 ���� ���������� �˰� �����͸� ����������ؼ�, ItemŬ������ �޾ƿ�.
    private Item storeItemData;
    private Inventory inventory;

    private CanvasGroup itemImg;

    private CraftItemUI craftItemUI;

    private void Awake()
    {
        inventory = UIManager.Instance.playerInventoryData;
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

        UIManager.Instance.playerInventoryData.GiveItemToEmptyInv(storeItemData, 1);

        itemImg.blocksRaycasts = false;

        // �ٽ� ��ĵ�ϱ�.
        craftItemUI.ReFreshCraftingUI();
    }

    


    

    public void CreateFromStore()
    {
        // ���⿡ ���ս� �� �����ָ��.
        switch (itemCode)
        {
            case 1001:
                inventory.RemoveItemFromInventory(3011, 1);
                inventory.RemoveItemFromInventory(3101, 1);
                break;
            case 1301:
                inventory.RemoveItemFromInventory(3011, 1);
                inventory.RemoveItemFromInventory(3101, 1);
                inventory.RemoveItemFromInventory(3001, 1);
                break;
        }
    }
}

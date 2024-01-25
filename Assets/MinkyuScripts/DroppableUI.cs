using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour, IDropHandler
{
    private int inventoryIndex;
    Image inventoryImg;

    private void Awake()
    {
        inventoryIndex = GetComponent<DraggableUI>().inventoryIndex;
        inventoryImg = GetComponent<Image>();
    }

    // ������ �ִ� ��ġ�� UI�� ���� �ִ� �Ͱ� �ٲ��־�ߴ�
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            // �̹��� �ӽ� ����ҷ� ������.
            UIManager.Instance.temporaryItemImg = inventoryImg.sprite;

            // �̹��� �巡�� �ߴ� ������ �ٲپ� �ֱ�.
            inventoryImg.sprite = eventData.pointerDrag.GetComponent<Image>().sprite;

            // �巡�� �ߴ� ������Ʈ�� �̹����� �ӽ�����ҿ� �ִ� �̹����� �ٲپ��ֱ�.
            eventData.pointerDrag.GetComponent<Image>().sprite = UIManager.Instance.temporaryItemImg;
            UIManager.Instance.temporaryItemImg = null;


            // ������Ʈ�� �̹����� ���İ� ����. �߰��� �����͵� �ű�.
            if (inventoryImg.sprite != null)
            {
                Color imageColor = inventoryImg.color;
                imageColor.a = 1f;
                inventoryImg.color = imageColor;

                // ������ ������� bool�� ����.
                UIManager.Instance.playerInventoryData.slots[inventoryIndex].isEmpty = false;

                // ������ �ӽ�����ҿ� �ø���.
                UIManager.Instance.takeTemporaryItemData = UIManager.Instance.playerInventoryData.slots[inventoryIndex].item;
                UIManager.Instance.takeTemporaryItemStack = UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack;
                UIManager.Instance.playerInventoryData.slots[inventoryIndex].item = null;
                UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack = 0;

                // ������ �޾ƿ���.
                UIManager.Instance.playerInventoryData.slots[inventoryIndex].item = UIManager.Instance.giveTemporaryItemData;
                UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack = UIManager.Instance.giveTemporaryItemStack;
                UIManager.Instance.giveTemporaryItemData = null;
                UIManager.Instance.giveTemporaryItemStack = 0;
                UIManager.Instance.playerInventoryData.invenSlot[inventoryIndex].ItemStackUIRefresh(UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack);
            }
        }
    }
}

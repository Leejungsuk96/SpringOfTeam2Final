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
        if (eventData.pointerDrag.GetComponent<Image>().sprite != null)
        {
            Debug.Log("����Ͼ");
            // ������ �ӽ�����ҿ� �ø���.
            UIManager.Instance.takeTemporaryItemData = UIManager.Instance.playerInventoryData.slots[inventoryIndex].item;
            UIManager.Instance.takeTemporaryItemStack = UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack;
            UIManager.Instance.playerInventoryData.slots[inventoryIndex].item = null;
            UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack = 0;

            // ������ ������� bool�� ����.
            UIManager.Instance.playerInventoryData.slots[inventoryIndex].isEmpty = false;

            // ������ �޾ƿ���.
            UIManager.Instance.playerInventoryData.slots[inventoryIndex].item = UIManager.Instance.giveTemporaryItemData;
            UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack = UIManager.Instance.giveTemporaryItemStack;
            UIManager.Instance.giveTemporaryItemData = null;
            UIManager.Instance.giveTemporaryItemStack = 0;

            // �̹��� ó��
            UIManager.Instance.StackUpdate(inventoryIndex);
        }
    }
}

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
            UIManager.instance.temporaryItemImg = inventoryImg.sprite;

            // �̹��� �巡�� �ߴ� ������ �ٲپ� �ֱ�.
            inventoryImg.sprite = eventData.pointerDrag.GetComponent<Image>().sprite;

            // �巡�� �ߴ� ������Ʈ�� �̹����� �ӽ�����ҿ� �ִ� �̹����� �ٲپ��ֱ�.
            eventData.pointerDrag.GetComponent<Image>().sprite = UIManager.instance.temporaryItemImg;
            UIManager.instance.temporaryItemImg = null;


            // ������Ʈ�� �̹����� ���İ� ����. �߰��� �����͵� �ű�.
            if (inventoryImg.sprite != null)
            {
                Color imageColor = inventoryImg.color;
                imageColor.a = 1f;
                inventoryImg.color = imageColor;

                // ������ ������� bool�� ����.
                UIManager.instance.playerInventoryData.slots[inventoryIndex].isEmpty = false;

                // ������ �ӽ�����ҿ� �ø���.
                UIManager.instance.takeTemporaryItemData = UIManager.instance.playerInventoryData.slots[inventoryIndex].item;
                UIManager.instance.playerInventoryData.slots[inventoryIndex].item = null;

                // ������ �޾ƿ���.
                UIManager.instance.playerInventoryData.slots[inventoryIndex].item = UIManager.instance.giveTemporaryItemData;
                UIManager.instance.giveTemporaryItemData = null;
            }
        }
    }
}

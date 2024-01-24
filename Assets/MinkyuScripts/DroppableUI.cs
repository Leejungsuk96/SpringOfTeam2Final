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
            MinUIManager.instance.temporaryItemImg = inventoryImg.sprite;

            // �̹��� �巡�� �ߴ� ������ �ٲپ� �ֱ�.
            inventoryImg.sprite = eventData.pointerDrag.GetComponent<Image>().sprite;

            // �巡�� �ߴ� ������Ʈ�� �̹����� �ӽ�����ҿ� �ִ� �̹����� �ٲپ��ֱ�.
            eventData.pointerDrag.GetComponent<Image>().sprite = MinUIManager.instance.temporaryItemImg;
            MinUIManager.instance.temporaryItemImg = null;


            // ������Ʈ�� �̹����� ���İ� ����. �߰��� �����͵� �ű�.
            if (inventoryImg.sprite != null)
            {
                Color imageColor = inventoryImg.color;
                imageColor.a = 1f;
                inventoryImg.color = imageColor;

                // ������ ������� bool�� ����.
                MinUIManager.instance.playerInventoryData.slots[inventoryIndex].isEmpty = false;

                // ������ �ӽ�����ҿ� �ø���.
                MinUIManager.instance.takeTemporaryItemData = MinUIManager.instance.playerInventoryData.slots[inventoryIndex].item;
                MinUIManager.instance.playerInventoryData.slots[inventoryIndex].item = null;

                // ������ �޾ƿ���.
                MinUIManager.instance.playerInventoryData.slots[inventoryIndex].item = MinUIManager.instance.giveTemporaryItemData;
                MinUIManager.instance.giveTemporaryItemData = null;
            }
        }
    }
}

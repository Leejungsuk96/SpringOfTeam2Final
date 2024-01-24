using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // �ڱⰡ ��� ° �κ��丮���� �˰����.
    [SerializeField] internal int inventoryIndex;

    private Transform canvas;
    private Transform previousParent;
    private RectTransform uiItemTransform;
    private CanvasGroup itemImg;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        uiItemTransform = GetComponent<RectTransform>();
        itemImg = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent;
        transform.SetParent(canvas);
        transform.SetAsLastSibling();

        // ������ �ӽ÷� �ðܵα�.
        MinUIManager.instance.giveTemporaryItemData = MinUIManager.instance.playerInventoryData.slots[inventoryIndex].item;
        MinUIManager.instance.playerInventoryData.slots[inventoryIndex].item = null;

        itemImg.alpha = 0.6f;
        itemImg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        uiItemTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(previousParent);
        uiItemTransform.position = previousParent.GetComponent<RectTransform>().position;

        itemImg.alpha = 1f;
        itemImg.blocksRaycasts = true;

        if (eventData.pointerDrag.GetComponent<Image>().sprite == null)
        {
            Color imageColor = eventData.pointerDrag.GetComponent<Image>().color;
            imageColor.a = 0f;
            eventData.pointerDrag.GetComponent<Image>().color = imageColor;

            // ������ ������� bool�� ����.
            MinUIManager.instance.playerInventoryData.slots[inventoryIndex].isEmpty = true;
        }
        else if (eventData.pointerDrag.GetComponent<Image>().sprite == eventData.pointerDrag.GetComponent<Image>().sprite)
        {
            // �ٸ� ���� ������ ��, �״�ζ�� �ٽ� ��������.
            MinUIManager.instance.playerInventoryData.slots[inventoryIndex].item = MinUIManager.instance.giveTemporaryItemData;
            MinUIManager.instance.giveTemporaryItemData = null;
        }
        else
        {
            MinUIManager.instance.playerInventoryData.slots[inventoryIndex].item = MinUIManager.instance.takeTemporaryItemData;
            MinUIManager.instance.takeTemporaryItemData = null;
        }
    }
}

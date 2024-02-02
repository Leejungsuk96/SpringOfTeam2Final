using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // �ڱⰡ ��� ° �κ��丮���� �˰����.
    // ���߿� Awake�� ��ȣ ã�ƿ��� ����.
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
        UIManager.Instance.giveTemporaryItemData = UIManager.Instance.playerInventoryData.slots[inventoryIndex].item;
        UIManager.Instance.giveTemporaryItemStack = UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack;
        UIManager.Instance.playerInventoryData.slots[inventoryIndex].item = null;
        UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack = 0;

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
            UIManager.Instance.playerInventoryData.slots[inventoryIndex].isEmpty = true;
        }
        // �ٸ� ���� ������ ��, �״�ζ�� �ٽ� ��������.
        else if (eventData.pointerDrag.GetComponent<Image>().sprite == eventData.pointerDrag.GetComponent<Image>().sprite)
        {
            UIManager.Instance.playerInventoryData.slots[inventoryIndex].item = UIManager.Instance.giveTemporaryItemData;
            UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack = UIManager.Instance.giveTemporaryItemStack;
            UIManager.Instance.giveTemporaryItemData = null;
            UIManager.Instance.giveTemporaryItemStack = 0;
        }
        // �Ű����ٸ� �ٲپ��� ������ ��������.
        else
        {
            UIManager.Instance.playerInventoryData.slots[inventoryIndex].item = UIManager.Instance.takeTemporaryItemData;
            UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack = UIManager.Instance.takeTemporaryItemStack;
            UIManager.Instance.takeTemporaryItemData = null;
            UIManager.Instance.giveTemporaryItemStack = 0;
        }
        UIManager.Instance.StackUpdate(inventoryIndex);
    }
}

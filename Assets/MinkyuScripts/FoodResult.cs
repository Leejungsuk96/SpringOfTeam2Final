using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodResult : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] internal int inventoryIndex;

    private Transform canvas;
    private Transform previousParent;
    private RectTransform uiItemTransform;
    private CanvasGroup itemImg;
    
    private int firstItem;
    private int secondItem;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        uiItemTransform = GetComponent<RectTransform>();
        itemImg = GetComponent<CanvasGroup>();

        firstItem = UIManager.Instance.playerInventoryData.slots[26].item.ItemType;
        secondItem = UIManager.Instance.playerInventoryData.slots[27].item.ItemType;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        // DraggableUI�� �ִ� ��ũ��Ʈ �Ϻθ� ���.
        previousParent = transform.parent;
        transform.SetParent(canvas);
        transform.SetAsLastSibling();

        itemImg.alpha = 0.6f;
        itemImg.blocksRaycasts = false;
        // ������� DraggableUI�� �ִ� ��ũ��Ʈ �Ϻ�


        // �ȿ� �����͵��� ����ִٸ�
        if ((firstItem == 8) && (secondItem == 8))
        {
            // �ش� �������� Stack�� 0���� ������
            if ((UIManager.Instance.playerInventoryData.slots[26].stack > 0) && (UIManager.Instance.playerInventoryData.slots[27].stack > 0))
            {
                Debug.Log("���տϷ�");
                FoodResultStack(true);
                // �ӽ�����ҷ� Item������ ����
                UIManager.Instance.giveTemporaryItemData = UIManager.Instance.playerInventoryData.slots[inventoryIndex].item;
                // �ӽ������ ���� +1
                UIManager.Instance.giveTemporaryItemStack++;
                // ������� ������ 0�̸�
                if ((UIManager.Instance.playerInventoryData.slots[26].stack == 0) || (UIManager.Instance.playerInventoryData.slots[27].stack == 0))
                {
                    UIManager.Instance.playerInventoryData.slots[inventoryIndex].item = null;
                    UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack = 0;
                    UIManager.Instance.StackUpdate(inventoryIndex);
                }
                // ���� ������ ���Ҵٸ�
                else
                {
                    UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack--;
                    UIManager.Instance.StackUpdate(inventoryIndex);
                }
            }
        }
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

    private void FoodResultStack(bool succes)
    {
        if (succes)
        {
            UIManager.Instance.playerInventoryData.slots[26].stack--;
            UIManager.Instance.playerInventoryData.slots[27].stack--;

            UIManager.Instance.StackUpdate(26);
            UIManager.Instance.StackUpdate(27);
        }
        else
        {
            UIManager.Instance.playerInventoryData.slots[26].stack++;
            UIManager.Instance.playerInventoryData.slots[27].stack++;

            UIManager.Instance.StackUpdate(26);
            UIManager.Instance.StackUpdate(27);
        }
    }
}

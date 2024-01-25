using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SetObject : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TileMapControl tileMapControl;
    private EquipObject equipObject;
    private List<SlotData> inventorySlot;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        equipObject = GetComponent<EquipObject>();
        inventorySlot = UIManager.Instance.playerInventoryData.slots;
    }
    // Start is called before the first frame update
    private void Start()
    {
        controller.OnSetEvent += BuildObject;
    }

    private void BuildObject()
    {
        for (int i = 0; i < 8; i++)
        {
            // �������� ���õǾ����� ������ ����.
            if ((inventorySlot[i].isChoose == true) && (inventorySlot[i].item != null) && (inventorySlot[i].item.ItemType != null))
            {
                
                // charactercontroller���� ��ġ������ ���������� �Ǵ��ϴ°� �ִ°�? ������ ���� ItemType���� �������־��� ���� ����� �Ʒ��� �Ǻ������� bool���� ����
                if (inventorySlot[i].item.ItemType == 1) // ��Ŭ���� ����� �� �ִ� �������ΰ�
                {
                    // ������ 0�̻��̸�.
                    if (inventorySlot[i].stack > 0)
                    {
                        // ���̶��
                        if (inventorySlot[i].item.ItemCode == 2101)
                        {
                            SetWall(i);
                        }
                        // �����̶��
                        else if (inventorySlot[i].item.ItemCode == 1701)
                        {
                            // �÷��̾� ü�� ȸ��
                            /*StackUpdate(i);*///���� ����.
                        }
                    }
                }
            }
            else
            {
                break;
            }
        }
        
    }

    private void SetWall(int i)
    {
        Vector2 mousPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(mousPosition, transform.position);
        if (distance < 2)
        {
            if (TilemapManager.instance.wallDictionary.ContainsKey(new Vector3Int(Mathf.FloorToInt(mousPosition.x), Mathf.FloorToInt(mousPosition.y), 0)))
            {
                return;
            }
            else
            {
                tileMapControl.CreateTile(Mathf.FloorToInt(mousPosition.x), Mathf.FloorToInt(mousPosition.y));
                // ���Ӱ� �������� Tile�� Dictionary�� �߰�.
                TilemapManager.instance.wallDictionary[new Vector3Int(Mathf.FloorToInt(mousPosition.x), Mathf.FloorToInt(mousPosition.y), 0)] = new TileInfo
                {
                    tile = tileMapControl.wallTile,
                    HP = 100f
                };
                StackUpdate(i);
                // ���� Ÿ���� �ٲ����� �Ϳ� ���� TileInfo�� �ٲ�����.
                Debug.Log("��ųʸ��� �߰�");
            }
        }
    }

    private void StackUpdate(int indexOfInventory)
    {
        inventorySlot[indexOfInventory].stack--;
        if (inventorySlot[indexOfInventory].stack == 0)
        {
            inventorySlot[indexOfInventory].item = null;
            inventorySlot[indexOfInventory].isEmpty = true;
            UIManager.Instance.playerInventoryData.invenSlot[indexOfInventory].ChangeInventoryImage(null);
            UIManager.Instance.playerInventoryData.invenSlot[indexOfInventory].OnOffImage(false);
            // ����ִ� ������ null�� �����.
            equipObject.heldItem.sprite = null;
        }
        UIManager.Instance.playerInventoryData.invenSlot[indexOfInventory].ItemStackUIRefresh(inventorySlot[indexOfInventory].stack);
    }

}

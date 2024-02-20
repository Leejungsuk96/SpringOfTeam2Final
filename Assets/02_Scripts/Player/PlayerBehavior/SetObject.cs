using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SetObject : MonoBehaviour, IUsePotion
{
    private CharacterController controller;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TileMapControl tileMapControl;
    private EquipObject equipObject;
    private List<SlotData> inventorySlot;
    private HealthSystem healthSystem;

    float PotionCoolTime = 2f;
    float MaxCoolTime = 10f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        equipObject = GetComponent<EquipObject>();
        inventorySlot = UIManager.Instance.playerInventoryData.slots;
        healthSystem = GetComponent<HealthSystem>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        controller.OnSetEvent += BuildObject;        
    }

    private void Update()
    {
        MaxCoolTime += Time.deltaTime;
    }

    private void BuildObject()
    {
        for (int i = 0; i < 8; i++)
        {
            // �������� ���õǾ����� ������ ����, ��Ŭ���� ����� �� �ִ� �������ΰ�
            if ((inventorySlot[i].isChoose == true) && (inventorySlot[i].item != null) && (inventorySlot[i].item.RightClick == true))
            {
                // ������ 0�̻��̸�.
                if (inventorySlot[i].stack > 0)
                {
                    // ������ Ÿ���� ���̶��
                    if (inventorySlot[i].item.ItemType == 1)
                    {
                        // TODO ���߿� ������ ���� Ÿ���� �ٸ��� ����.
                        SetWall(i);
                    }
                    // ���� �� �ִ� ���̶��
                    else if (inventorySlot[i].item.ItemType == 8)
                    {
                        UsePotionForChangeStats(i);
                        AudioManager.instance.PlaySffx(AudioManager.Sfx.PlayerEat);
                    }
                    // ���̶��
                    else if (inventorySlot[i].item.ItemType == 13)
                    {
                        // ���� ��ȯ�ϴ� ���� �ʿ�
                        Debug.Log("���ȯ");
                        SetField();
                        AudioManager.instance.PlaySffx(AudioManager.Sfx.PlayerHarvest);
                    }
                    // ���Ѹ���
                    else if (inventorySlot[i].item.ItemType == 14)
                    {
                        SetWater();
                        AudioManager.instance.PlaySffx(AudioManager.Sfx.PlayerPulling);
                    }
                    // �����̶��
                    else if (inventorySlot[i].item.ItemType == 15)
                    {
                        SetSeed(i);
                    }
                }
                break;
            }
            else
            {
                continue;
            }
        }

    }

    private void SetWall(int inventoryIndex)
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
                inventorySlot[inventoryIndex].stack--;
                UIManager.Instance.StackUpdate(inventoryIndex);
                // ����ִ� ������ null�� �����.
                equipObject.heldItem.sprite = null;
                // ���� Ÿ���� �ٲ����� �Ϳ� ���� TileInfo�� �ٲ�����.
                Debug.Log("��ųʸ��� �߰�");
            }
        }
    }

    private void SetField()
    {
        Vector2 mousPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(mousPosition, transform.position);

        if (distance < 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(mousPosition, Vector2.zero, 0.1f, 1 << LayerMask.NameToLayer("Field") | 1 << LayerMask.NameToLayer("Ground"));
            int layer = hit.collider.gameObject.layer;
            if (layer == LayerMask.NameToLayer("Field"))
            {
                Field field = hit.collider.gameObject.GetComponent<Field>();
                // ���߿� �۹��� �ڶ�� ���� �ʴٸ�. = �ڽĿ�����Ʈ�� �����ִٸ����� if�� ���� �ɾ��ֱ�.
                // �ڶ�� �ִٸ�
                if (field.isGrowing == true)
                {
                    // �� �ڶ��ٸ�
                    if (field.isGrowFinish)
                    {
                        ItemManager.instance.itemPool.ItemSpawn(field.seedData.ItemCode - 40 , hit.point);
                        field.ReadyHarvest();
                    }
                }
                // ���ڶ�� �ִٸ�.
                else
                {
                    //������ �ִٸ�
                    if (field.isSeed)
                    {
                        ItemManager.instance.itemPool.ItemSpawn(field.seedData.ItemCode, hit.point);
                    }
                    // �� ������Ʈ ���ֱ�.
                    hit.collider.gameObject.SetActive(false);
                }
            }
            else if (layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("�����Ϸ�");
                Vector3 spawnPosition = new Vector3(Mathf.FloorToInt(mousPosition.x) + 0.5f, Mathf.FloorToInt(mousPosition.y) + 0.5f);
                FarmManager.instance.FieldSpawn(spawnPosition);
            }
        }
    }

    private void SetWater()
    {
        Vector2 mousPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(mousPosition, transform.position);

        if (distance < 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(mousPosition, Vector2.zero, 0.1f, 1 << LayerMask.NameToLayer("Field"));
            if (hit)
            {
                Field field = hit.collider.gameObject.GetComponent<Field>();
                if (field.isWatering == false)
                {                    
                    field.isWatering = true;
                    field.CheckIsSeed();
                }
            }
        }
    }

    private void SetSeed(int inventoryIndex)
    {
        Vector2 mousPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(mousPosition, transform.position);

        if (distance < 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(mousPosition, Vector2.zero, 0.1f, 1 << LayerMask.NameToLayer("Field"));
            if (hit)
            {
                Field field = hit.collider.gameObject.GetComponent<Field>();
                if (field.isSeed == false)
                {
                    field.isSeed = true;
                    field.seedData = inventorySlot[inventoryIndex].item;
                    inventorySlot[inventoryIndex].stack--;
                    UIManager.Instance.StackUpdate(inventoryIndex);
                    field.CheckIsSeed();
                }
            }
        }
    }

    public void UsePotionForChangeStats(int i)
    {
        if(PotionCoolTime < MaxCoolTime)
        {
            MaxCoolTime = 0;
            healthSystem.ChangeHealth(inventorySlot[i].item.HP);
            healthSystem.ChangeHunger(inventorySlot[i].item.Hunger);            
            UIManager.Instance.playerInventoryData.slots[i].stack--;
            UIManager.Instance.StackUpdate(i);
        }             
    }
}

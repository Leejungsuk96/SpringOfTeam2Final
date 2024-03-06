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
    private HealthSystem healthSystem;

    float PotionCoolTime = 2f;
    float MaxCoolTime = 10f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        equipObject = GetComponent<EquipObject>();
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
            if ((UIManager.Instance.playerInventoryData.slots[i].isChoose == true) && (UIManager.Instance.playerInventoryData.slots[i].item != null) && (UIManager.Instance.playerInventoryData.slots[i].item.RightClick == true))
            {
                // ������ 0�̻��̸�.
                if (UIManager.Instance.playerInventoryData.slots[i].stack > 0)
                {
                    // ������ Ÿ���� ���̶��
                    if (UIManager.Instance.playerInventoryData.slots[i].item.ItemType == 1)
                    {
                        // TODO ���߿� ������ ���� Ÿ���� �ٸ��� ����.
                        SetWall(i);
                    }
                    // ���� �� �ִ� ���̶��
                    else if (UIManager.Instance.playerInventoryData.slots[i].item.ItemType == 8)
                    {
                        UsePotionForChangeStats(i);
                        AudioManager.instance.PlaySffx(AudioManager.Sfx.PlayerEat);
                    }
                    // ���̶��
                    else if (UIManager.Instance.playerInventoryData.slots[i].item.ItemType == 13)
                    {
                        // ���� ��ȯ�ϴ� ���� �ʿ�
                        Debug.Log("���ȯ");
                        SetField();
                    }
                    // ���Ѹ���
                    else if (UIManager.Instance.playerInventoryData.slots[i].item.ItemType == 14)
                    {
                        SetWater();
                    }
                    // �����̶��
                    else if (UIManager.Instance.playerInventoryData.slots[i].item.ItemType == 15)
                    {
                        SetSeed(i);
                    }
                    UIManager.Instance.playerInventoryData.StackUpdate(i);
                    if (UIManager.Instance.playerInventoryData.slots[i].item == null)
                    {
                        equipObject.heldItem.sprite = null;
                    }
                    else equipObject.heldItem.sprite = ItemManager.instance.GetSpriteByItemCode(UIManager.Instance.playerInventoryData.slots[i].item.ItemCode);
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
            // Ground�� Collider�� ��� Layer����ó���� �� ����ߴ�.
            RaycastHit2D hit = Physics2D.Raycast(mousPosition, Vector2.zero, 0.1f, 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("Field") | 1 << LayerMask.NameToLayer("Box") | 1 << LayerMask.NameToLayer("Item") | 1 << LayerMask.NameToLayer("Pyramid") | 1 << LayerMask.NameToLayer("Default"));

            // Ray�� �¾�����, �װ����� ���� �ִٴ� �Ŵ� ��ġ X
            if (hit)
            {
                Debug.Log(hit.transform.name);
                return;
            }
            // �ƹ��͵� ���� �ʾҴٸ�.
            else
            {
                Vector3Int cellPosition = new Vector3Int(Mathf.FloorToInt(mousPosition.x), Mathf.FloorToInt(mousPosition.y), 0);

                TilemapManager.instance.SetWallInfo(cellPosition, tileMapControl.wallTile);
                tileMapControl.CreateTile(cellPosition.x, cellPosition.y);

                UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack--;
            }
        }
    }

    private void SetField()
    {
        Vector2 mousPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(mousPosition, transform.position);

        if (distance < 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(mousPosition, Vector2.zero, 0.1f, 1 << LayerMask.NameToLayer("Field") | 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Pyramid") | 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Box"));
            
            if (!hit)
            {
                Debug.Log("�����Ϸ�");
                Vector3 spawnPosition = new Vector3(Mathf.FloorToInt(mousPosition.x) + 0.5f, Mathf.FloorToInt(mousPosition.y) + 0.5f);
                FarmManager.instance.FieldSpawn(spawnPosition);
                AudioManager.instance.PlaySffx(AudioManager.Sfx.PlayerHarvest);
            }
            else
            {
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
                            ItemManager.instance.itemPool.ItemSpawn(field.seedData.ItemCode - 40, hit.point);
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
                        AudioManager.instance.PlaySffx(AudioManager.Sfx.PlayerHarvest);
                        // �� ������Ʈ ���ֱ�.
                        hit.collider.gameObject.SetActive(false);
                    }
                }
                
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
                    AudioManager.instance.PlaySffx(AudioManager.Sfx.PlayerPulling);
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
                    field.seedData = UIManager.Instance.playerInventoryData.slots[inventoryIndex].item;
                    UIManager.Instance.playerInventoryData.slots[inventoryIndex].stack--;
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
            healthSystem.ChangeHealth(UIManager.Instance.playerInventoryData.slots[i].item.HP);
            healthSystem.ChangeHunger(UIManager.Instance.playerInventoryData.slots[i].item.Hunger);            
            UIManager.Instance.playerInventoryData.slots[i].stack--;
        }             
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Attack : MonoBehaviour
{
    private CharacterController controller;
    private CharacterStatHandler statsHandler;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float attackRange = 5.0f;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        statsHandler = GetComponent<CharacterStatHandler>();
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        controller.OnAttackEvent += PlayerAttack;
        controller.OnAttackEvent += PlayerMining;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {            
            PlayerAttack();
        }
    }

    private void PlayerAttack()
    {
        Debug.Log(statsHandler.CurrentStats.attackDamage);
        Quaternion playerRotation = transform.rotation;
        Vector2 forwardDirection = playerRotation * Vector3.up;
        Vector2 playerPosition = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPosition + forwardDirection * attackRange, attackRange);

        foreach (Collider2D collider in colliders)
        {
            HealthSystem enemyHealth = collider.GetComponent<HealthSystem>();

            if (enemyHealth != null)
            {
                enemyHealth.ChangeHealth(-10f);
            }
        }
    }


    private void PlayerMining()
    {
        Debug.Log("닿았다.");
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector3)mousePosition - gameObject.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.5f, 1 << 6);
        if (hit)
        {
            Debug.Log("닿았다2.");
            Vector3Int cellPosition = TilemapManager.instance.tilemap.WorldToCell(hit.point);
            if (cellPosition == null)
            {
                return;
            }
            else
            {
                if (!TilemapManager.instance.wallDictionary.ContainsKey(cellPosition)) return;

                if (TilemapManager.instance.wallDictionary[cellPosition].HP > 0f)
                {
                    TilemapManager.instance.wallDictionary[cellPosition].HP -= statsHandler.CurrentStats.miningAttack;
                    Debug.Log(TilemapManager.instance.wallDictionary[cellPosition].HP);

                    // 벽이 부서졌다면
                    if (TilemapManager.instance.wallDictionary[cellPosition].HP <= 0f)
                    {
                        TilemapManager.instance.tilemap.SetTile(TilemapManager.instance.tilemap.WorldToCell(cellPosition), null);
                        TilemapManager.instance.wallDictionary.Remove(cellPosition);
                        ItemManager.instacne.itemPool.ItemSpawn(2101, cellPosition);
                        // 타일의 지붕 없애기.
                        Vector3Int ceilingPosition = new Vector3Int(cellPosition.x, cellPosition.y + 1, 0);
                        if (TilemapManager.instance.ceilingTile.GetTile(ceilingPosition))
                        {
                            TilemapManager.instance.ceilingTile.SetTile(ceilingPosition, null);
                        }
                    }
                }
            }
        }
    }
}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
        
    //    Vector3Int mousePos = new Vector3Int(Mathf.FloorToInt(collision.transform.position.x), Mathf.FloorToInt(collision.transform.position.y), 0);
    //    TilemapManager.instance.wallDictionary.TryGetValue(mousePos, out TileInfo tileInfo);
 
    //    if(tileInfo.HP > 0)
    //    {
    //        tileInfo.HP -= statsHandler.CurrentStats.baseStatsSO.miningAttack;

    //        if(tileInfo.HP <= 0)
    //        {
    //            TilemapManager.instance.tilemap.SetTile(TilemapManager.instance.tilemap.WorldToCell(mousePos), null);
    //        }

    //    }            
        
    //}




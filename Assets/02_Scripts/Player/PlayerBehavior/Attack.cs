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
    private Animator playerAnimator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        statsHandler = GetComponent<CharacterStatHandler>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        controller.OnAttackEvent += PlayerAttack;
        controller.OnAttackEvent += PlayerMining;
    }

    private void PlayerAttack()
    {       
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector3)mousePosition - gameObject.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 2f, 1<<7);

        if (hit)
        {
            EffectManager.instance.effectPool.EffectIndex(EffectManager.EffectType.Attack, hit.point, direction);
            HealthSystem enemyHealth = hit.collider.GetComponent<HealthSystem>();
            if(enemyHealth != null)
            {
                enemyHealth.ChangeMHealth(-statsHandler.CurrentStats.attackDamage);
                UIManager.Instance.spawnDamageUI.SpawndamageTxt(hit.collider.transform.position, statsHandler.CurrentStats.attackDamage);
            }
        }
        //Quaternion playerRotation = transform.rotation;
        //Vector2 forwardDirection = playerRotation * Vector3.up;
        //Vector2 playerPosition = transform.position;
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPosition + forwardDirection * attackRange, attackRange);

        //foreach (Collider2D collider in colliders)
        //{
        //    if (collider.gameObject.CompareTag("Monster") || collider.gameObject.CompareTag("Boss"))
        //    {
        //        HealthSystem enemyHealth = collider.GetComponent<HealthSystem>();

        //        if (enemyHealth != null)
        //        {
        //            enemyHealth.ChangeMHealth(-statsHandler.CurrentStats.attackDamage);
        //            UIManager.Instance.spawnDamageUI.SpawndamageTxt(collider.transform.position, statsHandler.CurrentStats.attackDamage);   
        //            Debug.Log(statsHandler.CurrentStats.attackDamage + " ������ ����");
        //        }
        //    }
        //}
        playerAnimator.SetTrigger("Attack");
        AudioManager.instance.PlaySffx(AudioManager.Sfx.PlayerAttack);
    }


    private void PlayerMining()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (Vector3)mousePosition - gameObject.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.5f, 1 << 6);
        if (hit)
        {
            float extraDistance = 0.1f;
            Vector2 innerPoint = hit.point + (direction.normalized * extraDistance);
            Vector2 middlePoint = (hit.point + innerPoint) / 2;

            Vector3Int cellPosition = TilemapManager.instance.tilemap.WorldToCell(middlePoint);
            Debug.Log(cellPosition);
            if (cellPosition == null)
            {
                return;
            }
            else
            {
                if (TilemapManager.instance.wallDictionary[cellPosition].minMiningAttack <= statsHandler.CurrentStats.miningAttack)
                {


                    if (TilemapManager.instance.wallDictionary[cellPosition].HP > 0f)
                    {
                        TilemapManager.instance.wallDictionary[cellPosition].HP -= statsHandler.CurrentStats.miningAttack;
                        EffectManager.instance.effectPool.EffectIndex(EffectManager.EffectType.Mining, hit.point, direction);
                        Debug.Log(TilemapManager.instance.wallDictionary[cellPosition].HP);
                        AudioManager.instance.PlaySffx(AudioManager.Sfx.PlayerMining);
                        // ���� �μ����ٸ�
                        if (TilemapManager.instance.wallDictionary[cellPosition].HP <= 0f)
                        {
                            TilemapManager.instance.tilemap.SetTile(TilemapManager.instance.tilemap.WorldToCell(cellPosition), null);
                            ItemManager.instance.itemPool.ItemSpawn(2101, cellPosition);
                            // Ÿ���� ���� ���ֱ�.
                            Vector3Int ceilingPosition = new Vector3Int(cellPosition.x, cellPosition.y + 1, 0);
                            Vector3Int downCeilingPosition = new Vector3Int(cellPosition.x, cellPosition.y - 1, 0);
                            if (TilemapManager.instance.ceilingTile.GetTile(ceilingPosition))
                            {
                                TilemapManager.instance.ceilingTile.SetTile(ceilingPosition, null);
                            }
                            if (TilemapManager.instance.tilemap.GetTile(downCeilingPosition))
                            {
                                TilemapManager.instance.ceilingTile.SetTile(cellPosition, TilemapManager.instance.tileMapControl.ceilingTile);
                            }
                        }
                    }
                }
                else
                {
                    // ���⿡ �� ƨ������ ���� �ֱ�.
                    return;

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




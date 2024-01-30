using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    public GameObject itemPrefabs;
    private List<GameObject> itemPool;


    private void Awake()
    {
        itemPool = new List<GameObject>();
    }
    

    public void ItemSpawn(int itemCode, Vector3 spawnPosition)
    {
        GameObject selectItemPrefab = null;
        foreach (GameObject item in itemPool)
        {
            // 아이템 오브젝트가 꺼져있다면.
            if (!item.activeSelf)
            {
                selectItemPrefab = item;
                // 아이템 코드로 어떤 아이템인지 결정해주기.
                selectItemPrefab.GetComponent<PickUp>().itemIndex = itemCode;
                // 이미지도 여기서 결정해주면 좋을 것 같음. 아니면 이미지를 결정해주는 스크립트를 하나 만들어서 아이템 오브젝트에 컴퍼넌트로 넣어도 좋을듯.
                /*selectItemPrefab.GetComponent<SpriteRenderer>().sprite = ItemManager.instance.GetSpriteByItemCode(itemCode);*/
                // 완료.
                selectItemPrefab.transform.position = spawnPosition;
                selectItemPrefab.SetActive(true);
                break;
            }
        }

        // 다 켜져있다면
        if (!selectItemPrefab)
        {
            // 완료.
            selectItemPrefab = Instantiate(itemPrefabs, transform);
            selectItemPrefab.transform.position = spawnPosition;
            selectItemPrefab.GetComponent<PickUp>().itemIndex = itemCode;
            // 여기도 이미지 결정 추가.
            /*selectItemPrefab.GetComponent<SpriteRenderer>().sprite = ItemManager.instance.GetSpriteByItemCode(itemCode);*/
            itemPool.Add(selectItemPrefab);
        }
    }
}

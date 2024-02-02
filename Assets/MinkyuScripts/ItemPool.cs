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
            // ������ ������Ʈ�� �����ִٸ�.
            if (!item.activeSelf)
            {
                selectItemPrefab = item;
                // ������ �ڵ�� � ���������� �������ֱ�.
                selectItemPrefab.GetComponent<PickUp>().itemCode = itemCode;
                // �̹����� ���⼭ �������ָ� ���� �� ����. �ƴϸ� �̹����� �������ִ� ��ũ��Ʈ�� �ϳ� ���� ������ ������Ʈ�� ���۳�Ʈ�� �־ ������.
                /*selectItemPrefab.GetComponent<SpriteRenderer>().sprite = ItemManager.instance.GetSpriteByItemCode(itemCode);*/
                // �Ϸ�.
                selectItemPrefab.transform.position = spawnPosition;
                selectItemPrefab.SetActive(true);
                break;
            }
        }

        // �� �����ִٸ�
        if (!selectItemPrefab)
        {
            // �Ϸ�.
            selectItemPrefab = Instantiate(itemPrefabs, transform);
            selectItemPrefab.transform.position = spawnPosition;
            selectItemPrefab.GetComponent<PickUp>().itemCode = itemCode;
            // ���⵵ �̹��� ���� �߰�.
            /*selectItemPrefab.GetComponent<SpriteRenderer>().sprite = ItemManager.instance.GetSpriteByItemCode(itemCode);*/
            itemPool.Add(selectItemPrefab);
        }
    }
}

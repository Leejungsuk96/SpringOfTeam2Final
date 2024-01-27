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
                selectItemPrefab.GetComponent<PickUp>().itemIndex = itemCode;
                // �̹����� ���⼭ �������ָ� ���� �� ����. �ƴϸ� �̹����� �������ִ� ��ũ��Ʈ�� �ϳ� ���� ������ ������Ʈ�� ���۳�Ʈ�� �־ ������.
                selectItemPrefab.transform.position = spawnPosition;
                selectItemPrefab.SetActive(true);
                break;
            }
        }

        // �� �����ִٸ�
        if (!selectItemPrefab)
        {
            selectItemPrefab.GetComponent<PickUp>().itemIndex = itemCode;
            // ���⵵ �̹��� ���� �߰�.
            selectItemPrefab.transform.position = spawnPosition;
            selectItemPrefab = Instantiate(itemPrefabs, transform);
            itemPool.Add(selectItemPrefab);
        }
    }
}

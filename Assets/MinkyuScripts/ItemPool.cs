using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    public GameObject itemPrefabs;
    private List<GameObject> itemPool;

    public Dictionary<int, Sprite> spriteDictionary;

    private void Awake()
    {
        itemPool = new List<GameObject>();
        spriteDictionary = new Dictionary<int, Sprite>();
        SpriteMapping();
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
                selectItemPrefab.GetComponent<SpriteRenderer>().sprite = GetSpriteByItemCode(itemCode);
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
            selectItemPrefab.GetComponent<PickUp>().itemIndex = itemCode;
            // ���⵵ �̹��� ���� �߰�.
            selectItemPrefab.GetComponent<SpriteRenderer>().sprite = GetSpriteByItemCode(itemCode);
            itemPool.Add(selectItemPrefab);
        }
    }

    public Sprite GetSpriteByItemCode(int itemCode)
    {
        // Dictionary���� �ش� ��Ʈ ���� �����ϴ� Sprite�� ��������
        if (spriteDictionary.TryGetValue(itemCode, out Sprite sprite))
        {
            return sprite;
        }
        else
        {
            Debug.LogError("�ش� Ű���� �̹�������.");
            return null;
        }
    }


    public void SpriteMapping()
    {
        spriteDictionary.Add(2101, Resources.Load<Sprite>("ItemSprite/2101"));
    }
}

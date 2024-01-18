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

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        controller.OnSetEvent += BuildObject;
    }

    private void BuildObject()
    {
        Vector2 mousPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(mousPosition, transform.position);
        if (distance < 2)
        {
            if (!TilemapManager.instance.wallDictionary.ContainsKey(new Vector3Int(Mathf.FloorToInt(mousPosition.x), Mathf.FloorToInt(mousPosition.y), 0)))
            {
                tileMapControl.CreateTile(Mathf.FloorToInt(mousPosition.x), Mathf.FloorToInt(mousPosition.y));
                // ���Ӱ� �������� Tile�� Dictionary�� �߰�.
                TilemapManager.instance.wallDictionary[new Vector3Int(Mathf.FloorToInt(mousPosition.x), Mathf.FloorToInt(mousPosition.y), 0)] = new TileInfo
                {
                    tile = tileMapControl.wallTile,
                    HP = 100f
                };
                // ���� Ÿ���� �ٲ����� �Ϳ� ���� TileInfo�� �ٲ�����.
                Debug.Log("��ųʸ��� �߰�");
            }
        }
    }

}

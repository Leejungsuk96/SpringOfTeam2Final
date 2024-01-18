using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[System.Serializable]
public class TileInfo
{
    public TileBase tile;
    public float HP;

    // �ٸ� �Ӽ����� �ʿ信 ���� �߰�
}
public class TilemapManager : MonoBehaviour
{
    public static TilemapManager instance;

    public Tilemap tilemap;
    public Tilemap ceilingTile;
    public Dictionary<Vector3Int, TileInfo> wallDictionary = new Dictionary<Vector3Int, TileInfo>();

    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        // Bounds�� ����ü
        // �ش� Tile�� ũ�⸸ŭ �� for���� ������. tile�� ������ Dictionary�� �߰�. ���� TileInfo�� ���� �޴´�.
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
        {
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(cellPosition);

                if (tile != null)
                {
                    wallDictionary[cellPosition] = new TileInfo
                    {
                        tile = tile,
                        HP = 100f
                    };
                }
            }
        }
    }
    /*private void OnDrawGizmos()
    {
        BoundsInt bounds = tilemap.cellBounds;
        Gizmos.color = Color.green;
        for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
        {
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPosition);
                Gizmos.DrawWireCube(cellCenter, tilemap.cellSize);
            }
        }
    }*/
}

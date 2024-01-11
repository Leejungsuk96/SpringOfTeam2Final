using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapControl : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase newTile;

    public void CreateTile(int x, int y)
    {
        Vector3 cellPosition = new Vector3(x, y, 0);
        tilemap.SetTile(tilemap.WorldToCell(cellPosition), newTile);
        Debug.Log(cellPosition);
        // GetTile �� ����Ͽ� �ִ��� Ȯ��
    }

    public void DestroyTile(int x, int y)
    {
        Vector3 cellPosition = new Vector3(x, y, 0);
        tilemap.SetTile(tilemap.WorldToCell(cellPosition), null);
    }
}

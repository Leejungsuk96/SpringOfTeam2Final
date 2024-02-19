using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class WorldGenerator : MonoBehaviour
{
    public Tilemap terrainTilemap;
    public Tile[] forestTiles;
    public Tile[] caveTiles;
    public int[,] themeMap;

    public int mapWidth = 100;
    public int mapHeight = 100;
    public float noiseScale = 0.3f;
    public float themeScale = 0.05f; // �׸� ������ ����
    public float forestThemeThreshold = 0.4f; // �� �׸� �Ӱ谪 ����


    public event Action OnGenerationComplete;

    void Start()
    {
        GenerateWorld();
        OnGenerationComplete?.Invoke();
    }

    void GenerateWorld()
    {
        themeMap = new int[mapWidth, mapHeight];

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float forestNoise = Mathf.PerlinNoise((x + 10000) * themeScale, (y + 10000) * themeScale);
                float caveNoise = Mathf.PerlinNoise((x + 30000) * themeScale, (y + 30000) * themeScale);

                // ���� ���� ������ ������ �����ϴ� ���� ����
                if (forestNoise > caveNoise && forestNoise > 0.5)
                {
                    themeMap[x, y] = 0; // �� �������� ����
                }
                else if (caveNoise > forestNoise && caveNoise > 0.5)
                {
                    themeMap[x, y] = 1; // ���� �������� ����
                }
                // �߰� ���� �� �׸� ���� ����

                // �׸��� ���� Ÿ�� ��ġ
                Tile tileToPlace = themeMap[x, y] == 0 ? forestTiles[UnityEngine.Random.Range(0, forestTiles.Length)] :
                    themeMap[x, y] == 1 ? caveTiles[UnityEngine.Random.Range(0, caveTiles.Length)] : null;
                if (tileToPlace != null)
                {
                    terrainTilemap.SetTile(new Vector3Int(x - mapWidth / 2, y - mapHeight / 2, 0), tileToPlace);
                }
            }
        }
    }
}
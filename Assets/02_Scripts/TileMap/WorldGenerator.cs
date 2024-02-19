using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class WorldGenerator : MonoBehaviour
{
    public Tilemap terrainTilemap;
    public Tile[] forestTiles; // �� ������ Ÿ��
    public Tile[] caveTiles; // ���� ������ Ÿ��
    public int[,] themeMap;

    public int mapWidth = 100;
    public int mapHeight = 100;
    public float noiseScale = 0.3f;
    public float themeScale = 0.05f;

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
                float elevation = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);
                float themeNoise = Mathf.PerlinNoise(x * themeScale + 10000, y * themeScale + 10000);

                Tile[] tileArray;
                if (themeNoise < 0.5) // �� ����
                {
                    tileArray = forestTiles;
                    themeMap[x, y] = 0;
                }
                else // ���� ����
                {
                    tileArray = caveTiles;
                    themeMap[x, y] = 1;
                }

                int tileIndex = Mathf.FloorToInt(elevation * tileArray.Length);
                tileIndex = Mathf.Clamp(tileIndex, 0, tileArray.Length - 1);
                terrainTilemap.SetTile(new Vector3Int(x - mapWidth / 2, y - mapHeight / 2, 0), tileArray[tileIndex]);
            }
        }
    }
}

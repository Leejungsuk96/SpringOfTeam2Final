using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public Tilemap terrainTilemap; //Ÿ�ϸ�
    public Tile[] terrainTiles; //Ÿ�� �迭

    public int mapWidth = 100;
    public int mapHeight = 100;
    public float noiseScale = 0.3f;

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float noiseValue = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);
                int tileIndex = Mathf.FloorToInt(noiseValue * terrainTiles.Length);
                terrainTilemap.SetTile(new Vector3Int(x - mapWidth / 2, y - mapHeight / 2, 0), terrainTiles[tileIndex]);
            }
        }
    }
}
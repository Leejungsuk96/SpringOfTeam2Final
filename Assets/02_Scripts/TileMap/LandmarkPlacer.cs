using UnityEngine;

public class LandmarkPlacer : MonoBehaviour
{
    public GameObject[] forestLandmarks; // �� ������ ��ġ�� ���帶ũ �迭
    public GameObject[] caveLandmarks; // ���� ������ ��ġ�� ���帶ũ �迭
    public WorldGenerator worldGenerator;
    public float landmarkThreshold = 0.8f;

    void Start()
    {
        worldGenerator.OnGenerationComplete += PlaceLandmarks;
    }

    void OnDestroy()
    {
        worldGenerator.OnGenerationComplete -= PlaceLandmarks;
    }

    void PlaceLandmarks()
    {
        for (int x = 0; x < worldGenerator.mapWidth; x++)
        {
            for (int y = 0; y < worldGenerator.mapHeight; y++)
            {
                float noiseValue = Mathf.PerlinNoise(x * worldGenerator.noiseScale * 2, y * worldGenerator.noiseScale * 2);
                if (noiseValue > landmarkThreshold)
                {
                    Vector3 position = new Vector3(x - worldGenerator.mapWidth / 2, 0, y - worldGenerator.mapHeight / 2);
                    GameObject[] landmarkArray = null;

                    if (worldGenerator.themeMap[x, y] == 0) // �� ����
                    {
                        landmarkArray = forestLandmarks;
                    }
                    else if (worldGenerator.themeMap[x, y] == 1) // ���� ����
                    {
                        landmarkArray = caveLandmarks;
                    }

                    if (landmarkArray != null && landmarkArray.Length > 0)
                    {
                        GameObject landmarkPrefab = landmarkArray[Random.Range(0, landmarkArray.Length)];
                        Instantiate(landmarkPrefab, position, Quaternion.identity);
                    }
                }
            }
        }
    }
}

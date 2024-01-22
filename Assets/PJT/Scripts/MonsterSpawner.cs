using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform playerTransform;
    public float spawnRadius = 10f; // ���߿� �÷��̾� ȭ�� �ۿ����� �������� ���ؾ���
    public void SpawnMonsterNearPlayer()
    {
        Vector3 spawnPosition = playerTransform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = 0;
        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
    }
}
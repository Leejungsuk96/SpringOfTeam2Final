using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using UnityEngine;


// �����ϴ� ���
// 1. ������ �����Ͱ� ����
// 2. �����͸� ���̽����� ��ȯ
// 3. ���̽��� �ܺο� ����


// �ҷ����� ��
// 1. �ܺο� ����� ���̽��� ������
// 2. ���̽��� ������ ���·� ��ȯ
// 3. �ҷ��� �����͸� ���

public class GameData
{
    // �̸�, ����, ����, �������� ����    
    public Vector3 playerPosition;
    public Item[] slotItemData;

}
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public GameObject player;
    string path;
    string fileName = "SaveFile";
        
    GameData newGameData = new GameData();

    
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        path = Application.persistentDataPath + "/";
        LoadData();
    }
    // Start is called before the first frame update
    void Start()
    {
        player.transform.position = newGameData.playerPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData()
    {
        newGameData.playerPosition = player.transform.position;
        string data = JsonUtility.ToJson(newGameData);
        File.WriteAllText(path + fileName, data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + fileName);
        newGameData = JsonUtility.FromJson<GameData>(data);
    }
}

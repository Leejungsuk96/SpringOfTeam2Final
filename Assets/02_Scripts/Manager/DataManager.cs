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
    public SlotNum[] _slotNum;
    public List<SlotData> _slots;   
    
    public GameData(int num)
    {
        _slotNum = new SlotNum[num];
        _slots = new List<SlotData>();
        for (int i = 0; i < num; i++)
        {
            _slotNum[i] = new SlotNum();
            SlotData slot = new SlotData()
            {
                isEmpty = true,
                isChoose = false,
                item = null,
                stack = 0
            };
            _slots.Add(slot);
        }
    }
}
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public GameObject player;
    private Inventory inventory;
    string path;
    string fileName = "SaveFile";
        
    GameData newGameData = new GameData(26);

    
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        path = Application.persistentDataPath + "/";
        inventory = player.GetComponent<Inventory>();                
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        player.transform.position = newGameData.playerPosition;

        for (int i = 0; i < inventory.invenSlot.Length; i++)
        {
            inventory.invenSlot[i] = newGameData._slotNum[i];
            inventory.slots[i] = newGameData._slots[i];
        }
    }   
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData()
    {
        newGameData.playerPosition = player.transform.position;
        newGameData._slotNum = new SlotNum[inventory.invenSlot.Length];
        for (int i = 0; i < inventory.invenSlot.Length; i++)
        {
            newGameData._slotNum[i] = inventory.invenSlot[i];
            newGameData._slots[i] = inventory.slots[i];
        }
        string data = JsonUtility.ToJson(newGameData);
        File.WriteAllText(path + fileName, data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + fileName);
        newGameData = JsonUtility.FromJson<GameData>(data);
    }
}

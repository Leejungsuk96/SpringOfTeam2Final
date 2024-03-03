using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using UnityEngine;
using UnityEngine.Tilemaps;


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
    
    public Tilemap tilemap;
    public Tilemap ceilingTile;
    
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

    public Tilemap _tilemap;
    public Tilemap _ceilingtile;
        
    

    
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
        if(File.Exists(path + fileName))
        {
            LoadData();
        }            
    }   
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData()
    {
        // ������ ������ ��üȭ
        GameData newGameData = new GameData(26); 
        
        // �÷��̾� ��ġ ����
        newGameData.playerPosition = player.transform.position;

        // �� ����
        BoundsInt boundsWall = _tilemap.cellBounds;
        TileBase[] allWallTiles = _tilemap.GetTilesBlock(boundsWall);
        Tilemap newWallTilemap = _tilemap;
        newWallTilemap.ClearAllTiles();
        newWallTilemap.SetTilesBlock(boundsWall, allWallTiles);
        newGameData.tilemap = newWallTilemap;

        BoundsInt boundscell = _ceilingtile.cellBounds;
        TileBase[] allCellTiles = _ceilingtile.GetTilesBlock(boundscell);
        Tilemap newCellTilemap = _ceilingtile;
        newCellTilemap.ClearAllTiles();
        newCellTilemap.SetTilesBlock(boundscell, allCellTiles);
        newGameData.ceilingTile = newCellTilemap;
        

        // ������ ���� ����
        for (int i = 0; i < 26; i++)
        {
            newGameData._slotNum[i] = inventory.invenSlot[i];
            newGameData._slots[i] = inventory.slots[i];
        }

        // ���̽� �����ͷ� ��ȯ
        string data = JsonUtility.ToJson(newGameData);
        File.WriteAllText(path + fileName, data);
    }

    public void LoadData()
    {
        // ���̽� ������ �ڵ�� ��ȯ 
        string data = File.ReadAllText(path + fileName);
        GameData loadGameData = JsonUtility.FromJson<GameData>(data);

        // �÷��̾� ��ġ ����
        player.transform.position = loadGameData.playerPosition;

        // �� ���� ����
        TilemapManager.instance.tilemap = loadGameData.tilemap;
        TilemapManager.instance.ceilingTile = loadGameData.ceilingTile;                    
        
        // ������ ���� ����
        for (int i = 0; i < 26; i++)
        {
            inventory.invenSlot[i] = loadGameData._slotNum[i];
            inventory.slots[i] = loadGameData._slots[i];
            inventory.StackUpdate(i);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject Player;
    // �÷��̾� ���� UI
    [SerializeField] private Slider HPSlider;
    [SerializeField] private Slider HungerSilder;

    [SerializeField] private TextMeshProUGUI HpTxt;
    [SerializeField] private TextMeshProUGUI HungerTxt;

    // ���� ���� UI
    [SerializeField] private GameObject BossHPUI;
    [SerializeField] private Slider BossHPBar;
    [SerializeField] private TextMeshProUGUI BossName;

    private HealthSystem playerHealthSystem;
    public SpawnDamageUI spawnDamageUI;

    // �κ��丮 ������
    public Inventory playerInventoryData;
    public Item takeTemporaryItemData;
    public Item giveTemporaryItemData;
    public int takeTemporaryItemStack;
    public int giveTemporaryItemStack;

    //���â �ؽ�Ʈ
    public TextMeshProUGUI playerStatHP;
    public TextMeshProUGUI playerStatATK;
    public TextMeshProUGUI playerStatDEF;
    public TextMeshProUGUI playerStatMOV;
    public TextMeshProUGUI playerStatSight;

    private void Awake()
    {
        Instance = this;

        //playerHealthSystem = Player.GetComponent<HealthSystem>();
        //playerHealthSystem.OnDamage += UpdateUI;
        //playerHealthSystem.OnHeal += UpdateUI;
    }

    private void UpdateUI()
    {
        HPSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.MaxHealth;
        HpTxt.text = playerHealthSystem.CurrentHealth.ToString() + "/" + playerHealthSystem.MaxHealth.ToString();

        HungerSilder.value = playerHealthSystem.CurrentHunger / playerHealthSystem.MaxHunger;
        HungerTxt.text = playerHealthSystem.CurrentHunger.ToString() + "/" + playerHealthSystem.MaxHealth.ToString();

    }

    public void StackUpdate(int indexOfInventory)
    {
        if (playerInventoryData.slots[indexOfInventory].stack == 0)
        {
            playerInventoryData.slots[indexOfInventory].item = null;
            playerInventoryData.slots[indexOfInventory].isEmpty = true;
            playerInventoryData.invenSlot[indexOfInventory].ChangeInventoryImage(0);
            playerInventoryData.invenSlot[indexOfInventory].OnOffImage(0);
        }
        else if (playerInventoryData.slots[indexOfInventory].stack > 0)
        {
            playerInventoryData.slots[indexOfInventory].isEmpty = false;
            playerInventoryData.invenSlot[indexOfInventory].ChangeInventoryImage(playerInventoryData.slots[indexOfInventory].item.ItemCode);
            playerInventoryData.invenSlot[indexOfInventory].OnOffImage(1f);
        }
        playerInventoryData.invenSlot[indexOfInventory].ItemStackUIRefresh(playerInventoryData.slots[indexOfInventory].stack);
    }

    public void UpdatePlayerStatTxt()
    {
        CharacterStatHandler statHandler = Player.GetComponent<CharacterStatHandler>();
        playerStatHP.text = statHandler.CurrentStats.maxHP.ToString();
        playerStatATK.text = statHandler.CurrentStats.attackDamage.ToString();
        playerStatDEF.text = statHandler.CurrentStats.defense.ToString();
        playerStatMOV.text = statHandler.CurrentStats.speed.ToString();
        playerStatSight.text = 1.ToString();
        
    }
}

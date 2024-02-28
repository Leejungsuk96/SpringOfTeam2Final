using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestBoard : MonoBehaviour
{
    public TMP_Text questShortCutTxt;
    public Image questShortCutImg;
    public Button clearBtn;
    public Button cancelBtn;
    public CanvasGroup questShortCutAlpha;

    internal QuestInfo questInfo;
    private Inventory inventory;
    private ItemManager itemManager;

    private void Awake()
    {
        QuestManager.instance.OnCheckQuestRequest += CheckRequst;
        QuestManager.instance.OnQuestAccepted += OnQuestAccepted;
    }

    private void Start()
    {
        inventory = UIManager.Instance.playerInventoryData;
        itemManager = ItemManager.instance;
    }

    public void ShortCutFadeOut()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(questShortCutAlpha.DOFade(0f, 1f).OnComplete(DeactiveThis));

        // ������ ����
        sequence.Play();
    }

    public void ShortCutFadeIn()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(questShortCutAlpha.DOFade(1f, 1f));

        sequence.Play();
    }

    private void OnQuestAccepted()
    {
        if (questInfo != null)
        {
            gameObject.SetActive(true);
            CheckRequst();
            ShortCutFadeIn();
        }
        else return;
    }

    public void CheckRequst()
    {
        if (questInfo != null)
        {
            questShortCutTxt.text = $"{inventory.ReturnStackInInventory(questInfo.requestItem.ItemCode)}/{questInfo.requestCount}";
            questShortCutImg.sprite = itemManager.GetSpriteByItemCode(questInfo.requestItem.ItemCode);

            if (inventory.CheckStackAmount(questInfo.requestItem.ItemCode, questInfo.requestCount))
            {
                // ��ư �̹��� �ٲٰ� ������ ���ֱ�.
                clearBtn.interactable = true;

                ColorBlock colors = clearBtn.colors;
                colors.colorMultiplier = 2f;
                clearBtn.colors = colors;
            }
            else
            {
                clearBtn.interactable = false;

                ColorBlock colors = clearBtn.colors;
                colors.colorMultiplier = 1f;
                clearBtn.colors = colors;
            }
        }
        else Debug.LogError("QuestBoard�� ����� ����Ʈ ������ �����ϴ�.");
    }

    // ClearBtn�� ����.
    public void GetReward()
    {
        inventory.RemoveItemFromInventory(questInfo.requestItem.ItemCode, questInfo.requestCount);
        inventory.GiveItemToEmptyInv(questInfo.rewardItem, questInfo.rewardCount);

        QuestManager.instance.CheckAllQuestRequest();
        ShortCutFadeOut();
    }

    private void DeactiveThis()
    {
        gameObject.SetActive(false);
    }
}

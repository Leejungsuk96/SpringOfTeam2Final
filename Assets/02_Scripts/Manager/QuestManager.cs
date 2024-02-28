using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestInfo
{
    public Item requestItem;
    public int requestCount;

    public Item rewardItem;
    public int rewardCount;
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public QuestShortCut questShortCut;
    internal int questSlotCount;
    internal QuestInfo questInfo;

    public event Action OnQuestAccepted;
    public event Action OnCheckQuestRequest;

    private void Awake()
    {
        instance = this;

        questShortCut = FindObjectOfType<QuestShortCut>();

        if (questShortCut != null)
        {
            questSlotCount = questShortCut.questBoards.Length;
        }
    }

    public void SetQuestOnQuestBoard()
    {
        for (int i = 0; i < questSlotCount; i++)
        {
            if (questShortCut.questBoards[i].questInfo == null)
            {
                if (questInfo != null)
                {
                    questShortCut.questBoards[i].questInfo = questInfo;
                    questInfo = null;
                    AcceptQuest();
                    break;
                }
                else
                {
                    Debug.LogError("����Ʈ ������ ����Ʈ â���� �ȳѾ�ɴϴ�.");
                }
            }
        }
    }

    public void AcceptQuest()
    {
        // ����Ʈ�� �޾��� �� �̺�Ʈ ȣ��
        OnQuestAccepted?.Invoke();
    }

    public void CheckAllQuestRequest()
    {
        OnCheckQuestRequest?.Invoke();
    }
}

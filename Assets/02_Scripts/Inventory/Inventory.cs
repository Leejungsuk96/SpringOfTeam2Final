using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<SlotData> slots = new();// SlotData�� ����Ʈ�� ���������.
    public SlotNum[] invenSlot;

    public void Start()
    {
        for(int i = 0; i < invenSlot.Length; i++)
        {
            SlotData slot = new()// ��ü �ʱ�ȭ �ܼ�ȭ
            {
                isEmpty = true,
                isChoose = false,
                item = null,
                stack = 0
            };
            slots.Add(slot);
        }
    }
}

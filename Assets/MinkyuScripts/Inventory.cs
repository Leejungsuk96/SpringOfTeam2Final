using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<SlotData> slots= new List<SlotData>();// SlotData�� ����Ʈ�� ���������.
    public SlotNum[] invenSlot;

    public void Start()
    {
        for(int i = 0; i < invenSlot.Length; i++)
        {
            SlotData slot = new SlotData();
            slot.isEmpty = true;
            slot.slotObj = invenSlot[i].gameObject;
            slots.Add(slot);
        }
    }
}

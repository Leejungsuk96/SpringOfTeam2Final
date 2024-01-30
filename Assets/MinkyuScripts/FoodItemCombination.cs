using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodItemCombination : MonoBehaviour, IDragHandler, IDropHandler
{
    private Dictionary<Tuple<int, int>, Item> foodRecipe;


    private void Start()
    {
        StartCoroutine(FoodRecipe());
    }
    public void CheckRecipe()
    {
        // 음식 조합의 칸 두개의 Item데이터가 비어있지 않을 때
        if (UIManager.Instance.playerInventoryData.slots[26].item.ItemType == 8 && UIManager.Instance.playerInventoryData.slots[27].item.ItemType == 8)
        {
            int ingredient1 = UIManager.Instance.playerInventoryData.slots[26].item.ItemCode;
            int ingredient2 = UIManager.Instance.playerInventoryData.slots[27].item.ItemCode;

            Tuple<int, int> key = Tuple.Create(ingredient1, ingredient2);

            Debug.Log(key);

            if (foodRecipe.ContainsKey(key))
            {
                Item resultFood = foodRecipe[key];
                UIManager.Instance.playerInventoryData.slots[28].item = resultFood;
                UIManager.Instance.playerInventoryData.slots[28].isEmpty = false;
                UIManager.Instance.playerInventoryData.invenSlot[28].GetComponent<Image>().sprite = ItemManager.instance.GetSpriteByItemCode(UIManager.Instance.playerInventoryData.slots[28].item.ItemCode);

            }
            else
            {
                Debug.Log("레시피 없음");
            }
        }
        else
        {
            UIManager.Instance.playerInventoryData.slots[28].item = null;
        }
    }

    public IEnumerator FoodRecipe()
    {
        yield return new WaitForSeconds(1f);

        foodRecipe = new Dictionary<Tuple<int, int>, Item>();
        foodRecipe.Add(Tuple.Create(1703, 1713), ItemManager.instance.items[1723]);
        foodRecipe.Add(Tuple.Create(1713, 1703), ItemManager.instance.items[1723]);
        Debug.Log(foodRecipe[Tuple.Create(1713, 1703)]);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("조합 검토");
        CheckRecipe();
    }

    public void OnDrag(PointerEventData eventData)
    {
        CheckRecipe();
    }
}

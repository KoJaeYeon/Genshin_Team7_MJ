using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemPanel : MonoBehaviour,IItemPanel
{
    TextMeshProUGUI itemName;
    Image itemImage;
    TextMeshProUGUI itemType;
    TextMeshProUGUI itemDescription;
    private void Awake()
    {
        itemName = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        itemImage = transform.GetChild(1).GetComponent<Image>();
        itemType = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemDescription = transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void UpdateItemPanel(int id)
    {
        Item item = ItemDatabase.Instance.GetItem(id);
        itemName.text = item.itemName;
        itemImage.sprite = ItemDatabase.Instance.GetItemSprite(id);
        if (id < 100) itemType.text = "무기";
        else if (id < 200) itemType.text = "성유물";
        else itemType.text = itemType.text = "소재";
        Debug.Log(item.description);
        itemDescription.text = item.description;
    }
}

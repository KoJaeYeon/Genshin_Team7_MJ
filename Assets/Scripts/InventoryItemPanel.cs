using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemPanel : MonoBehaviour,IItemPanel
{
    Image background;
    Image itemNameImage;
    TextMeshProUGUI itemName;
    Image itemImage;
    TextMeshProUGUI itemType;
    TextMeshProUGUI itemDescription;
    private void Awake()
    {
        background = GetComponent<Image>();
        itemNameImage = transform.GetChild(1).GetComponent<Image>();
        itemName = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        itemImage = transform.GetChild(2).GetComponent<Image>();
        itemType = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        itemDescription = transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void UpdateItemPanel(int id, CharacterItemSprite c)
    {
        Item item = ItemDatabase.Instance.GetItem(id);
        background.color = ItemDatabase.Instance.GetColor(item.star);

        switch (item.star)
        {
            case 1:
                itemNameImage.color = new Color(113 / 255f, 138 / 255f, 138 / 255f);
                break;
            case 2:
                itemNameImage.color = new Color(42 / 255f, 143 / 255f, 114 / 255f);
                break;
            case 3:
                itemNameImage.color = new Color(81 / 255f, 127 / 255f, 203 / 255f);
                break;
            case 4:
                itemNameImage.color = new Color(161 / 255f, 86 / 255f, 224 / 255f);
                break;
            case 5:
                itemNameImage.color = new Color(188 / 255f, 105 / 255f, 50 / 255f);
                break;
            default:
                itemNameImage.color = Color.white;
                break;
        }

        itemName.text = item.itemName;
        itemImage.sprite = ItemDatabase.Instance.GetItemSprite(id);
        if (id < 100) itemType.text = "무기";
        else if (id < 200) itemType.text = "성유물";
        else itemType.text = itemType.text = "소재";
        itemDescription.text = item.description;
    }
}

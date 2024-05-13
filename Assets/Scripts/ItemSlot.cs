using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    int key;
    int id;
    string name;
    int count;
    Image itemImage;
    Image characterImage;
    TextMeshProUGUI text;

    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        characterImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }
    public void InitUpdateSlot(int key, Item item)
    {
        this.key = key;
        this.id = item.id;
        this.name = item.id.ToString();
        this.count = item.count;
        this.itemImage.sprite = ItemDatabase.Instance.GetItemSprite(id);
        if (item.isEquip == false)
        {
            text.text = item.count.ToString();
        }
        else if (item.weaponDamage > 0)
        {
            text.text = "Lv.90";
        }
        else
        {
            text.text = "+20";
        }
    }
    public void UpdateSlot(Item item)
    {
        this.count = item.count;
        text.text = item.count.ToString();
    }
    public void OwnerChange(Character character)
    {

    }
}

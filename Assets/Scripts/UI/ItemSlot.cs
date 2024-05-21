using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    int key;
    int id;
    string itemName;
    int count;
    CharacterItemSprite character;
    Image backgrondColor;
    Image itemImage;
    TextMeshProUGUI text;
    Image characterImage;

    private void Awake()
    {
        backgrondColor = transform.GetChild(0).GetComponent<Image>();
        itemImage = transform.GetChild(0). GetChild(1).GetComponent<Image>();
        text = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        characterImage = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Image>();
    }
    public void InitUpdateSlot(int key, Item item)
    {
        this.key = key;
        this.id = item.id;
        this.itemName = item.itemName;
        this.count = item.count;
        this.itemImage.sprite = ItemDatabase.Instance.GetItemSprite(id);
        if (item.isEquip == false)
        {
            text.text = item.count.ToString();
        }
        else if ((int)item.equipType < 5)
        {
            text.text = "Lv.90";
        }
        else
        {
            text.text = "+20";
        }
        backgrondColor.color = ItemDatabase.Instance.GetColor(item.star);
        characterImage.transform.parent.gameObject.SetActive(false);
    }
    public void UpdateSlot(Item item)
    {
        this.count = item.count;
        text.text = item.count.ToString();
    }

    public void ShowData()
    {
        UIManager.Instance.showDataUpdate(id, character);
    }

    public void OwnerChange(CharacterItemSprite character)
    {
        this.character = character;
        switch (character)
        {
            case CharacterItemSprite.None:
                characterImage.transform.parent.gameObject.SetActive(false);                
                break;
            default:
                characterImage.sprite = ItemDatabase.Instance.GetCharacterSprite(character);
                characterImage.transform.parent.gameObject.SetActive(true);
                break;
        }        
    }

    public void UnEquip()
    {

    }

    public EqiupType GetRelicType()
    {
        return ItemDatabase.Instance.GetRelicType(id);
    }
}

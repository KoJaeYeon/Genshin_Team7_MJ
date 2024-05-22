using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicItemPanel : MonoBehaviour,IItemPanel
{
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemType;
    TextMeshProUGUI valueType;
    TextMeshProUGUI value;

    GameObject equipButton;
    GameObject unEquipButton;
    private void Awake()
    {
        itemName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemType = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        valueType = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        value = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();

        equipButton = transform.parent.GetChild(2).gameObject;
        unEquipButton = transform.parent.GetChild(3).gameObject;
    }
    public void UpdateItemPanel(int id, CharacterItemSprite characterFromSlot)
    {
        Item item = ItemDatabase.Instance.GetItem(id);
        itemName.text = item.itemName;
        switch (id % 5)
        {
            case 0:
                itemType.text = "�̼��� �հ�";
                valueType.text = "����";
                value.text = $"{item.value.ToString()}%";
                break;
            case 1:
                itemType.text = "������ ��";
                valueType.text = "HP";
                value.text = $"{item.value.ToString()}";
                break;
            case 2:
                itemType.text = "������ ����";
                valueType.text = "���ݷ�";
                value.text = $"{item.value.ToString()}";
                break;
            case 3:
                itemType.text = "�ð��� ��";
                valueType.text = "ü��";
                value.text = $"{item.value.ToString()}%";
                break;
            case 4:
                itemType.text = "������ ����";
                valueType.text = "���ݷ�";
                value.text = $"{item.value.ToString()}%";
                break;
        }
        if(EquipManager.Instance.character.Equals(characterFromSlot))
        {
            equipButton.SetActive(false);
            unEquipButton.SetActive(true);
        }
        else
        {
            equipButton.SetActive(true);
            unEquipButton.SetActive(false);
        }
    }
}

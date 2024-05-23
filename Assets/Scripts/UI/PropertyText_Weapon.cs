using TMPro;
using UnityEngine;

public class PropertyText_Weapon: MonoBehaviour
{
    TextMeshProUGUI itemName;
    TextMeshProUGUI itemType;
    TextMeshProUGUI value;

    private void Awake()
    {
        itemName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        itemType = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        value = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        EquipStats equipStats = EquipManager.Instance.GetEquip(EquipManager.Instance.character);
        Item item = InventoryManager.Instance.GetWeaponItem(equipStats.itemSlotKeys[0]);
        Debug.Log(item.itemName);
        itemName.text = item.itemName;
        switch (item.equipType)
        {
            case EqiupType.Claymore:
                itemType.text = "��հ�";
                break;
            case EqiupType.Catalyst:
                itemType.text = "����";
                break;
            case EqiupType.Bow:
                itemType.text = "Ȱ";
                break;
            case EqiupType.Pole:
                itemType.text = "�庴��";
                break;
            case EqiupType.Sword:
                itemType.text = "�Ѽհ�";
                break;
        }
        value.text = item.value.ToString();
    }
}

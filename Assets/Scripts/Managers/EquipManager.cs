using Unity.VisualScripting;
using UnityEngine;

public class EquipManager : Singleton<EquipManager>
{
    public CharacterItemSprite character = CharacterItemSprite.None;
    public EquipStats beidou_Equip;
    public EquipStats kokomi_Equip;
    public EquipStats wrio_Equip;
    public EquipStats yoimiya_Equip;
    public ItemSlot itemSlot;

    private void Awake()
    {
        beidou_Equip =new EquipStats();
        kokomi_Equip =new EquipStats();
        wrio_Equip = new EquipStats();
        yoimiya_Equip = new EquipStats();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            for(ForEach)
            //Debug.Log(beidou_Equip.weaponDamage);
            //Debug.Log(beidou_Equip.flowerHealth);
            //Debug.Log(beidou_Equip.featherDamage);
            //Debug.Log(beidou_Equip.sandTime_HelathPercent);
            //Debug.Log(beidou_Equip.trohphy_AttackPercent);
            //Debug.Log(beidou_Equip.crown_defencePercent);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(yoimiya_Equip.weaponDamage);
            Debug.Log(yoimiya_Equip.flowerHealth);
            Debug.Log(yoimiya_Equip.featherDamage);
            Debug.Log(yoimiya_Equip.sandTime_HelathPercent);
            Debug.Log(yoimiya_Equip.trohphy_AttackPercent);
            Debug.Log(yoimiya_Equip.crown_defencePercent);
        }
    }

    public void Equip()
    {
        itemSlot.UnEquip();
        itemSlot.OwnerChange(CharacterItemSprite.None); //사용할 아이템 선 장착해제
        int id = itemSlot.GetId();
        Item item = ItemDatabase.Instance.GetItem(id);
        EquipStats equipStats = GetEquip(character);
        int tempKey;
        switch(item.equipType)
        {
            case EqiupType.Flower:
                tempKey = equipStats.itemSlotKeys[1];
                equipStats.flowerHealth = item.value;
                equipStats.itemSlotKeys[1] = itemSlot.GetKey();
                break;
            case EqiupType.Feather:
                tempKey = equipStats.itemSlotKeys[2];
                equipStats.featherDamage = item.value;
                equipStats.itemSlotKeys[2] = itemSlot.GetKey();
                break;
            case EqiupType.SandTime:
                tempKey = equipStats.itemSlotKeys[3];
                equipStats.sandTime_HelathPercent = item.value;
                equipStats.itemSlotKeys[3] = itemSlot.GetKey();
                break;
            case EqiupType.Trophy:
                tempKey = equipStats.itemSlotKeys[4];
                equipStats.trohphy_AttackPercent = item.value;
                equipStats.itemSlotKeys[4] = itemSlot.GetKey();
                break;
            case EqiupType.Crown:
                tempKey = equipStats.itemSlotKeys[5];
                equipStats.crown_defencePercent = item.value;
                equipStats.itemSlotKeys[5] = itemSlot.GetKey();
                break;
            default:
                tempKey = equipStats.itemSlotKeys[0];
                equipStats.weaponDamage = item.value;
                equipStats.itemSlotKeys[0] = itemSlot.GetKey();
                break;
        }
        itemSlot.OwnerChange(character);
        if(tempKey != -1)
        {
            if((int)item.equipType < 5)
            {
                ItemSlot previous_ItemSlot = InventoryManager.Instance.GetWeaponItemSlot(tempKey);
                UnEquip(previous_ItemSlot.character, item.equipType);
                previous_ItemSlot.OwnerChange(CharacterItemSprite.None);
            }
            else
            {
                ItemSlot previous_ItemSlot = InventoryManager.Instance.GetRelicItemSlot(tempKey);
                UnEquip(previous_ItemSlot.character, item.equipType);
                previous_ItemSlot.OwnerChange(CharacterItemSprite.None);
            }
        }
    }

    public void Equip(ItemSlot itemSlot)
    {
        int id = itemSlot.GetId();
        Item item = ItemDatabase.Instance.GetItem(id);
        EquipStats equipStats = GetEquip(itemSlot.character);
        switch (item.equipType)
        {
            case EqiupType.Flower:
                equipStats.flowerHealth = item.value;
                equipStats.itemSlotKeys[1] = itemSlot.GetKey();
                break;
            case EqiupType.Feather:
                equipStats.featherDamage = item.value;
                equipStats.itemSlotKeys[2] = itemSlot.GetKey();
                break;
            case EqiupType.SandTime:
                equipStats.sandTime_HelathPercent = item.value;
                equipStats.itemSlotKeys[3] = itemSlot.GetKey();
                break;
            case EqiupType.Trophy:
                equipStats.trohphy_AttackPercent = item.value;
                equipStats.itemSlotKeys[4] = itemSlot.GetKey();
                break;
            case EqiupType.Crown:
                equipStats.crown_defencePercent = item.value;
                equipStats.itemSlotKeys[5] = itemSlot.GetKey();
                break;
            default:
                equipStats.weaponDamage = item.value;
                equipStats.itemSlotKeys[0] = itemSlot.GetKey();
                break;
        }
    }

    public void UnEquip()
    {
        UnEquip(character, itemSlot.GetEquipType());
    }

    public void UnEquip(CharacterItemSprite character, EqiupType equipType)
    {
        Debug.Log("unequip1" + this.character + character);
        if (this.character == character) return;
        Debug.Log("unequip2" + this.character + character);
        EquipStats equipStats = GetEquip(character);
        {
            switch (equipType)
            {
            case EqiupType.Flower:
                    equipStats.flowerHealth = 0;
                    equipStats.itemSlotKeys[1] = -1;
                    break;
                case EqiupType.Feather:
                    equipStats.featherDamage = 0;
                    equipStats.itemSlotKeys[2] = -1;
                    break;
                case EqiupType.SandTime:
                    equipStats.sandTime_HelathPercent = 0;
                    equipStats.itemSlotKeys[3] = -1;
                    break;
                case EqiupType.Trophy:
                    equipStats.trohphy_AttackPercent = 0;
                    equipStats.itemSlotKeys[4] = -1;
                    break;
                case EqiupType.Crown:
                    equipStats.crown_defencePercent = 0;
                    equipStats.itemSlotKeys[5] = -1;
                    break;
                default:
                    equipStats.weaponDamage = 0;
                    equipStats.itemSlotKeys[0] = -1;
                    break;
                }
            }
    }

    public EquipStats GetEquip(CharacterItemSprite character)
    {
        switch(character)
        {
            case CharacterItemSprite.Beidou:
                return beidou_Equip;
            case CharacterItemSprite.Kokomi:
                return kokomi_Equip;
            case CharacterItemSprite.Wriothesley:
                return wrio_Equip;
            case CharacterItemSprite.Yoimiya:
                return yoimiya_Equip;
            default:
                return default;
        }
        
    }
}

public class EquipStats
{
    public float weaponDamage;
    public float flowerHealth;
    public float featherDamage;
    public float sandTime_HelathPercent;
    public float trohphy_AttackPercent;
    public float crown_defencePercent;

    public int[] itemSlotKeys;

    public EquipStats()
    {
        itemSlotKeys = new int[6] {-1,-1,-1,-1,-1,-1 };
    }
}

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

    public Character[] playerCharacter;

    private void Awake()
    {
        beidou_Equip =new EquipStats();
        kokomi_Equip =new EquipStats();
        wrio_Equip = new EquipStats();
        yoimiya_Equip = new EquipStats();

        playerCharacter = new Character[4];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            foreach(int i in beidou_Equip.itemSlotKeys)
            {
                Debug.Log(i);
            }
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

    public void Equip() // 선택한 아이템 슬롯 아이템 장착
    {
        if (itemSlot.character == character) return; // 착용주인 아이템을 착용 누르는 경우가 생기면 취소
        CharacterItemSprite previousCharacter = itemSlot.character; // 사용할 아이템 사용중이었던 캐릭터
        if(itemSlot.character != CharacterItemSprite.None)//사용할 아이템 선 장착해제
        {
            UnEquip(itemSlot.character, itemSlot.GetEquipType()); //캐릭터의 해당 부위 장착 해제
        }
 
        int id = itemSlot.GetId(); // 착용하고자 하는 장비의 ID
        Item item = ItemDatabase.Instance.GetItem(id); // 착용하고자 하는 장비의 아이템
        EquipStats equipStats = GetEquip(character); // 현재 설정된 캐릭터의 장비슬롯
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
                ChangeWeapon(character, item.id);
                break;
        }
        itemSlot.OwnerChange(character);
        if(tempKey != -1) // 비어있는 장비랑 교체할 때 조건 설정
        {
            if((int)item.equipType < 5)
            {
                ItemSlot previous_ItemSlot = InventoryManager.Instance.GetWeaponItemSlot(tempKey);
                previous_ItemSlot.OwnerChange(previousCharacter);
                Equip(previous_ItemSlot);
            }
            else
            {
                ItemSlot previous_ItemSlot = InventoryManager.Instance.GetRelicItemSlot(tempKey);
                previous_ItemSlot.OwnerChange(previousCharacter);
                Equip(previous_ItemSlot);
            }
        }
        itemSlot.ShowData();
    }

    public void Equip(ItemSlot itemSlot) // 빈슬롯에 소유자가 있는 아이템 적용
    {
        if (itemSlot.character == CharacterItemSprite.None) return; // 소유자가 없으면 취소
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
                ChangeWeapon(itemSlot.character, item.id);
                break;
        }
    }

    

    public void UnEquip()
    {
        UnEquip(character, itemSlot.GetEquipType());
    }

    public void UnEquip(CharacterItemSprite character, EqiupType equipType) // 아이템 장착해제
    {
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
        itemSlot.OwnerChange(CharacterItemSprite.None);
        itemSlot.ShowData();
    }

    public void ChangeWeapon(CharacterItemSprite character, int id)
    {
        if(character == CharacterItemSprite.Beidou && id >=3 &&  id <=5)
        {
            playerCharacter[0].SwitchWeapon(id - 3);
        }
        else if (character == CharacterItemSprite.Kokomi && id >= 9 && id <= 11)
        {
            playerCharacter[1].SwitchWeapon(id - 9);
        }
        else if (character == CharacterItemSprite.Yoimiya && id >= 6 && id <= 8)
        {
            playerCharacter[3].SwitchWeapon(id - 6);
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

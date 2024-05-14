using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public enum DefenceType
{
    Head,
    Attack,
    Clock,
    Grail,
    Crown
}

[System.Serializable]
public class Item
{
    int _id;
    string _itemName;
    int _count;
    bool _isEquip;
    float _weaponDamage;
    DefenceType _defenceType;
    float _value;
    int indexId;

    #region Property
    public int id { get => _id; set => _id = value; }
    public int count { get => _count; set => _count = value; }
    public bool isEquip { get => _isEquip; set => _isEquip = value; }
    public float weaponDamage { get => _weaponDamage; set => _weaponDamage = value; }
    public DefenceType defenceType {  get => _defenceType; set => _defenceType = value; }
    public float value { get => _value; set => _value = value; }
    #endregion

    public Item(int id, string itemname , int count, bool isEquip, float weaponDamage, DefenceType defenceType, float value)
    {
        _id = id;
        _itemName = itemname;
        _count = count;
        _isEquip = isEquip;
        _weaponDamage = weaponDamage;
        _defenceType = defenceType;
        _value = value;
    }

    public Item(Item item)
    {
        _id = item._id;
        _count = item._count;
        _isEquip = item._isEquip;
        _weaponDamage = item._weaponDamage;
        _defenceType = item._defenceType;
        _value = item._value;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        if (!(obj is Item)) return false;
        Item other = (Item)obj;
        if (_isEquip == false && _id == other._id) return true;
        else if(indexId == other.indexId) return true;
        return false;
    }

}

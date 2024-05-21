using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum EqiupType
{
    ClayMore,
    Catalyst,
    Bow,
    Pole,
    Sword,
    Flower,
    Feather,
    SandTime,
    Trophy,
    Crown
}

[System.Serializable]
public class Item
{
    int _id;
    string _itemName;
    int _count;
    bool _isEquip;
    EqiupType _equipType;
    float _value;
    string _description;
    int indexId;

    #region Property
    public int id { get => _id; set => _id = value; }
    public string itemName { get => _itemName; set => _itemName = value; }
    public int count { get => _count; set => _count = value; }
    public bool isEquip { get => _isEquip; set => _isEquip = value; }
    public EqiupType equipType {  get => _equipType; set => _equipType = value; }
    public float value { get => _value; set => _value = value; }
    public string description { get => _description; set => _description = value; }
    #endregion

    public Item(int id, string itemname , int count, bool isEquip, EqiupType defenceType, float value, string description)
    {
        _id = id;
        _itemName = itemname;
        _count = count;
        _isEquip = isEquip;
        _equipType = defenceType;
        _value = value;
        _description=description;
    }

    public Item(Item item)
    {
        _id = item._id;
        _itemName = item._itemName;
        _count = item._count;
        _isEquip = item._isEquip;
        _equipType = item._equipType;
        _value = item._value;
        _description = item._description;
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

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

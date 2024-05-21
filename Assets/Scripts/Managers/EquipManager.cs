using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : Singleton<EquipManager>
{
    public EquipStats beidou_Equip;
    public EquipStats kokomi_Equip;
    public EquipStats wrio_Equip;
    public EquipStats yoimiya_Equip;
}

public struct EquipStats
{
    public float weaponDamage;
    public float flowerHealth;
    public float featherDamage;
    public float sandTime_HelathPercent;
    public float trohphy_AttackPercent;
    public float crown_defencePercent;
}

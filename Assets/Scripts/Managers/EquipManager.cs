using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : Singleton<EquipManager>
{
    EquipStats beidou_Equip;
    EquipStats kokomi_Equip;
    EquipStats wrio_Equip;
    EquipStats yoimiya_Equip;
}

public struct EquipStats
{
    float weaponDamage;
    float flowerHealth;
    float featherDamage;
    float sandTime_HelathPercent;
    float trohphy_AttackPercent;
    float crown_defencePercent;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Melee,
    Ranged
}

public abstract class Weapon : MonoBehaviour
{
    public Character character;
    public abstract void UseWeapon();
}

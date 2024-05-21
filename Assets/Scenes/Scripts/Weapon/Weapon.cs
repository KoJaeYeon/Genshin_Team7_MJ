using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int damage;
    public float attackDuration;
    public abstract void UseWeapon();
}

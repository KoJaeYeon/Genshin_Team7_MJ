using UnityEngine;

public enum CharacterType
{
    Melee,
    Ranged
}

public abstract class Weapon : MonoBehaviour
{
    public Character character;
    public Element element = Element.Normal;
    public abstract void UseWeapon();
}

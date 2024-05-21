using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData_")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int level;
    public float baseHp;
    public float baseAtk;
    public float baseDef;  

    public Vector3 controllerCenter;
    public float controllerRadius;
    public float controllerHeight;
}

using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData_")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int level;
    public int hp;
    public int atk;
    public RuntimeAnimatorController controller;
}

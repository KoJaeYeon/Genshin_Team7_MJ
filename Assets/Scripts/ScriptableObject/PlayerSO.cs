using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData_")]
public class PlayerSO : ScriptableObject
{
    public string playerName;
    public int playerLevel;
    public int currentHealth;
    public int maxHealth;
    public int currentEnergy;
    public int maxEnergy;
    public int currentStamina;
    public int maxStamina;
    public int sprintStaminaCost;
    public int flyStaminaCost;
    public int recoveryStaminaCost;
    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5.335f;
    public int maxDefaultAttackCount;

    public void Initialize(string name, int level, int initialHealth, int initialEnergy)
    {
        playerName = name;
        playerLevel = level;
        currentHealth = initialHealth;
        maxHealth = initialHealth;
        currentEnergy = 0;
        maxEnergy = initialEnergy;
        maxStamina = 110;
        sprintStaminaCost = 10;
        recoveryStaminaCost = 5;
    }

    public void Damage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth-amount, 0);
    }

    public void GetEnergy(int amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
    }

    public void UseStamina(int amount)
    {
        currentStamina = Mathf.Max(currentStamina - amount, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCSVData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Health { get; set; }
    public float AttackPower { get; set; }
    public float Speed { get;set; }
    public float Defence { get; set; }
    public Element element { get; set; }

    public EnemyCSVData(int id, string name, float health, float attackPower, float speed, float defence, Element element)
    {
        Id=id;
        Name=name;
        Health=health;
        AttackPower=attackPower;
        Speed=speed;
        Defence=defence;
        this.element=element;
    }
}

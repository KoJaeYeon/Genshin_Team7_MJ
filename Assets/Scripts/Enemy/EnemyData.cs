using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCSVData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public float Health { get; set; }
    public float Power { get; set; }
    public float Speed { get;set; }
    public float TraceDistance { get; set; }
    public Element Element { get; set; }
    public float Paralyzation { get;set; }
    public float Defence { get; set; }
    public Color Color { get; set; }


    public EnemyCSVData(string id, string name, float health, float attackPower, 
        float speed, float traceDistance, Element element, float paralyzation, float defence, Color color)
    {
        Id=id;
        Name=name;
        Health=health;
        Power=attackPower;
        Speed=speed;
        TraceDistance=traceDistance;
        Element=element;
        Paralyzation=paralyzation;
        Defence=defence;
        Color=color;    
    }
}

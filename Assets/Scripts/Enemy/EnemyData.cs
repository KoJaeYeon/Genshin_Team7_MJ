using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCSVData { }
public class EnemyBaseData : EnemyCSVData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public float Health { get; set; }
    public float Power { get; set; }
    public float Speed { get; set; }
    public float Defense { get; set; }

    public EnemyBaseData(string id, string name, float health, 
        float power, float speed, float defense)
    {
        Id = id;
        Name = name;
        Health = health;
        Power = power;
        Speed = speed;
        Defense = defense;
    }
}

public class EnemyElementData : EnemyCSVData
{
    public string Id { get; set; }
    public Element Element { get;set; }
    public Color Color { get; set; }
    public int DropCount { get; set; }

    public EnemyElementData(string id, Element element,Color color, int dropCount)
    {
        Id = id;
        Element = element;
        Color = color;
        DropCount = dropCount;
    }
}

public class EnemyTraceData : EnemyCSVData
{
    public string Id { get; set; }
    public float TraceDistance { get; set; }
    public float StopDistance { get; set; }
    public float AgentStopDistance { get; set; }
    public float NextMoveTime {  get; set; }

    public EnemyTraceData(string id, float traceDistance, 
        float stopDistance, float agentstopDistance, float nextMoveTime)
    {
        Id = id;
        TraceDistance = traceDistance;
        StopDistance = stopDistance;
        AgentStopDistance = agentstopDistance;
        NextMoveTime = nextMoveTime;
    }
}

public class EnemyOverlapData : EnemyCSVData
{
    public string Id { get; set; }
    public List<Vector3> BoxList { get; set; }

    public EnemyOverlapData(string id, List<Vector3> boxList)
    {
        Id = id;
        BoxList = boxList;
    }
}
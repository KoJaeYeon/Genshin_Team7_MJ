using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatternName
{
    AndriusParalyzation,
    AndriusWalk,
    AndriusAttack,
    JumpAttack,
    ClawAttack,
    ChargeAttack,
    StampAttack,
    DriftAttack,
    HowlAttack
}

public class AndriusPatternData { }
public class AndriusParalyzationData : AndriusPatternData
{
    public string Id { get; set; }
    public float ChangeTime { get; set; }
    public float ParalyzationValue { get; set; }
    public AndriusParalyzationData(string id, float changeTime,
        float paralyzationValue)
    {
        Id = id;
        ChangeTime = changeTime;
        ParalyzationValue = paralyzationValue;
    }
}

public class AndriusWalkData : AndriusPatternData
{
    public string Id { get; set; }
    public float Speed { get; set; }
    public float WalkTime { get; set; }
    public AndriusWalkData(string id, float speed, float walkTime)
    {
        Id = id;
        Speed = speed;
        WalkTime = walkTime;
    }
}

public class AndriusAttackData : AndriusPatternData
{
    public string Id { get; set; }
    public enum DataList
    {
        JumpDelay = 1,
        ChargeDelay,
        MeleeDistance,
        JumpDistance,
        ChargeDistance,
        MoveDistance,
        Turn_rightAngle,
        Turn_leftAngle,
        Back_leftAngle,
        Back_rightAngle,
        Back_Distance
    }
    
    public Dictionary<DataList, float> Data { get; set; }

    public float GetData(DataList dataList)
    {
        if (Data.TryGetValue(dataList, out float data))
        {
            return data;
        }
        else
            return 0f;
    }
}

public class AndriusJumpData : AndriusPatternData
{
    public string Id { get; set; }
    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float SkillDamage { get; set; }
    public AndriusJumpData(string id, float moveSpeed, float rotationSpeed,
        float skillDamage)
    {
        Id = id;
        MoveSpeed = moveSpeed;
        RotationSpeed = rotationSpeed;
        SkillDamage = skillDamage;
    }
}

public class AndriusClawData : AndriusPatternData
{
    public string Id { get; set; }
    public float SkillDamage { get; set; }
    public AndriusClawData(string id, float skillDamage)
    {
        Id = id;
        SkillDamage = skillDamage;
    }
}

public class AndriusChargeData : AndriusPatternData
{
    public string Id { get; set; }
    public float RotationSpeed { get; set; }
    public float ChargeTime { get; set; }
    public float MaxAngle { get; set; }
    public float SkillDamage { get; set; }
    public AndriusChargeData(string id, float rotationSpeed,
        float chargeTime, float maxAngle, float skillDamage)
    {
        Id = id;
        RotationSpeed = rotationSpeed;
        ChargeTime = chargeTime;
        MaxAngle = maxAngle;
        SkillDamage = skillDamage;
    }
}

public class AndriusStampData : AndriusPatternData
{
    public string Id { get; set; }
    public float RotationSpeed { get; set; }
    public float MaxNormalizedTime { get; set; }
    public float SkillDamage { get; set; }
    public AndriusStampData(string id, float rotationSpeed,
        float maxNormalizedTime, float skillDamage)
    {
        Id = id;
        RotationSpeed = rotationSpeed;
        MaxNormalizedTime = maxNormalizedTime;
        SkillDamage = skillDamage;
    }
}

public class AndriusDriftData : AndriusPatternData
{
    public string Id { get; set; }
    public float SkillDamage { get; set; }
    public AndriusDriftData(string id, float skillDamage)
    {
        Id = id;
        SkillDamage = skillDamage;
    }
}

public class AndriusHowlData : AndriusPatternData
{
    public string Id { get; set; }
    public enum DataList
    {
        SKillDamage_howl,
        SKillDamage_ice
    }

    public Dictionary<DataList,float> Data { get; set; }  
    
}
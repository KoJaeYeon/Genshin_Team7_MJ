using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndriusCSVData { }
public class AndriusParalyzationData : AndriusCSVData
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

public class AndriusWalkData : AndriusCSVData
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

public class AndriusAttackData : AndriusCSVData
{
    public string Id { get; set; }
    public Dictionary<AttackDataList, float> Data { get; set; }
    public float GetData(AttackDataList dataList)
    {
        if (Data.TryGetValue(dataList, out float data))
        {
            return data;
        }
        else
            return 0f;
    }
}

public class AndriusJumpData : AndriusCSVData
{
    private float _chargeTime;
    private float _maxAngle;
    private float _maxNormalizedTime;

    public string Id { get; set; }
    public float MoveSpeed { get; set; }
    public float RotationSpeed { get; set; }
    public float SkillDamage { get; set; }

    public AndriusJumpData(string id, float moveSpeed, float rotationSpeed, 
        float chargeTime, float maxAngle ,float maxNormalizedTime, float skillDamage)
    {
        Id = id;
        MoveSpeed = moveSpeed;
        RotationSpeed = rotationSpeed;
        _chargeTime = chargeTime;
        _maxAngle = maxAngle;
        _maxNormalizedTime = maxNormalizedTime;
        SkillDamage = skillDamage;
    }
}

public class AndriusClawData : AndriusCSVData
{
    private float _moveSpeed;
    private float _rotationSpeed;
    private float _chargeTime;
    private float _maxAngle;
    private float _maxNormalizedTime;

    public string Id { get; set; }
    public float SkillDamage { get; set; }

    public AndriusClawData(string id, float moveSpeed, float rotationSpeed,
        float chargeTime, float maxAngle, float maxNormalizedTime, float skillDamage)
    {
        Id = id;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
        _chargeTime = chargeTime;
        _maxAngle = maxAngle;
        _maxNormalizedTime = maxNormalizedTime;
        SkillDamage = skillDamage;
    }
}

public class AndriusChargeData : AndriusCSVData
{
    private float _moveSpeed;
    private float _maxNormalizedTime;

    public string Id { get; set; }
    public float RotationSpeed { get; set; }
    public float ChargeTime { get; set; }
    public float MaxAngle { get; set; }
    public float SkillDamage { get; set; }

    public AndriusChargeData(string id, float moveSpeed, float rotationSpeed,
        float chargeTime, float maxAngle, float maxNormalizedTime, float skillDamage)
    {
        Id = id;
        _moveSpeed = moveSpeed;
        RotationSpeed = rotationSpeed;
        ChargeTime = chargeTime;
        MaxAngle = maxAngle;
        _maxNormalizedTime = maxNormalizedTime;
        SkillDamage = skillDamage;
    }
}

public class AndriusStampData : AndriusCSVData
{
    private float _moveSpeed;
    private float _chargeTime;
    private float _maxAngle;

    public string Id { get; set; }
    public float RotationSpeed { get; set; }
    public float MaxNormalizedTime { get; set; }
    public float SkillDamage { get; set; }
    public AndriusStampData(string id, float moveSpeed, float rotationSpeed,
        float chargeTime, float maxAngle, float maxNormalizedTime, float skillDamage)
    {
        Id = id;
        _moveSpeed = moveSpeed;
        RotationSpeed = rotationSpeed;
        _chargeTime = chargeTime;
        _maxAngle = maxAngle;
        MaxNormalizedTime = maxNormalizedTime;
        SkillDamage = skillDamage;
    }
}

public class AndriusDriftData : AndriusCSVData
{
    private float _moveSpeed;
    private float _rotationSpeed;
    private float _chargeTime;
    private float _maxAngle;
    private float _maxNormalizedTime;

    public string Id { get; set; }
    public float SkillDamage { get; set; }
    public AndriusDriftData(string id, float moveSpeed, float rotationSpeed,
        float chargeTime, float maxAngle, float maxNormalizedTime, float skillDamage)
    {
        Id = id;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
        _chargeTime = chargeTime;
        _maxAngle = maxAngle;
        _maxNormalizedTime = maxNormalizedTime;
        SkillDamage = skillDamage;
    }
}

public class AndriusHowlData : AndriusCSVData
{
    private float _moveSpeed;
    private float _rotationSpeed;
    private float _chargeTime;
    private float _maxAngle;
    private float _maxNormalizedTime;

    public string Id { get; set; }
    public Dictionary<DataList,float> Data { get; set; } 

    public float GetData(DataList dataList)
    {
        if(Data.TryGetValue(dataList, out var data))
        {
            return data;
        }
        else
        {
            Debug.Log("HowlData를 가져오지 못했습니다.");
            return 0;
        }
    }
    
    public AndriusHowlData(string id, float moveSpeed, float rotationSpeed,
        float chargeTime, float maxAngle, float maxNormalizedTime)
    {
        Id = id;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
        _chargeTime = chargeTime;
        _maxAngle = maxAngle;
        _maxNormalizedTime = maxNormalizedTime;
    }
}
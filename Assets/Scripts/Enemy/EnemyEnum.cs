
public enum AttackDataList
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
    Back_Distance,
    MeleeAngle,
    DriftAngle
}

public enum DataList
{
    SKillDamage_howl,
    SKillDamage_ice
}

public enum AndriusPattern
{
    Idle,
    Move,
    Attack,
    Jump,
    Claw,
    Charge,
    Stamp,
    Drift,
    Howl
}

public enum EnemyLayer
{
    isAlive = 3,
    isDead = 6
}

public enum Element
{
    Normal,
    Fire,
    Ice,
    Lightning,
    Water,

    Null
}

public enum MonsterType
{
    Fire = 1,
    Ice,
    Normal,
    Lightning,
    Andrius

}

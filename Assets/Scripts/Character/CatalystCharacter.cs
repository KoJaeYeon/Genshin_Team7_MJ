using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CatalystCharacter : Character
{
    public enum AttackType { Melee, Ranged }
    public AttackType attackType;

    protected override void Start()
    {
        base.Start();
        SetAttackParameters();
    }

    private void SetAttackParameters()
    {
        if(attackType == AttackType.Melee)
        {
            detectionRange = 5.0f;
            detectionAngle = 90.0f;
        }
        else if(attackType == AttackType.Ranged)
        {
            detectionRange = 15.0f;
            detectionAngle = 60.0f;
        }

    }
    public override void Attack()
    {
        List<GameObject> enemies = DetectedEnemiesInRange();

        if(enemies.Count > 0 )
        {
            FaceTarget(enemies[0].transform.position);
            weapons[currentWeaponIndex].UseWeapon();
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}

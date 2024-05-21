using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedCatalystCharacter : CatalystCharacter
{
    public GameObject projectilePrefab;
    public Transform launchPoint;
    public float projectileSpeed;

    public override void Attack()
    {
        base.Attack();
        ShootProjectile();
    }

    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, launchPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = launchPoint.forward * projectileSpeed;
    }

    protected override void Update()
    {
        base.Update();
    }
}

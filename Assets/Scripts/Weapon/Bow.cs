using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bow : Weapon
{
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    private bool isAiming = false;

    public override void UseWeapon()
    {
        if (isAiming)
        {
            PerformAimedShot();
        }
        else
        {
            PerformNormalShot();
        }
    }

    private void PerformNormalShot()
    {
        Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    }

    private void PerformAimedShot()
    {
        Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            isAiming = !isAiming;
            if (isAiming)
            {

            }
            else
            {

            }
        }
    }
}

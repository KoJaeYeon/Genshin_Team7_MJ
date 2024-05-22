using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bow : Weapon
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
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
        
    }

    private void PerformAimedShot()
    {
        
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

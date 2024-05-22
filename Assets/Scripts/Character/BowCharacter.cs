using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowCharacter : Character
{
    //public Transform arrowSpawnPoint;
    //public GameObject arrowPrefab;
    //public float rotationSpeed = 5f;
    //public LayerMask enemyLayer;

    //private Transform currentTarget;
    //private bool isAiming = false;
    //public GameObject crosshair;
    //private Transform player;
    protected override void Start()
    {
        characterType = CharacterType.Ranged;
        base.Start();
        foreach(var weapon in weapons)
        {
            if(weapon is Bow)
            {
                weapon.gameObject.SetActive(true);
                currentWeaponIndex = System.Array.IndexOf(weapons, weapon);
                break;
            }
        }

        //if(crosshair != null)
        //{
        //    crosshair.SetActive(true);
        //}

        //player = transform.parent;
    }

    //private void Update()
    //{
    //    if (isAiming) AimMode();
    //    else NormalMode();

    //    if (Keyboard.current.rKey.wasPressedThisFrame)
    //    {
    //        isAiming = !isAiming;
    //        if (isAiming) EnterAimMode();
    //        else ExitAimMode();
    //    }
    //}

    //private void NormalMode()
    //{
    //    DetectTargets();

    //    if (currentTarget != null && Keyboard.current.rKey.wasPressedThisFrame)
    //    {
    //        StartCoroutine(FaceTargetAndShoot());
    //    }
    //}

    //private void AimMode(Vector2 lookInput)
    //{
    //    float mouseX = lookInput.x;
    //    float mouseY = lookInput.y;

    //    Vector3 rotation = player.eulerAngles;
    //    rotation.x -= mouseX;
    //    rotation.y += mouseY;
    //    player.eulerAngles = rotation;

    //    if (Mouse.current.leftButton.wasPressedThisFrame)
    //    {
    //        ShootArrow();
    //    }
    //}

    //private void DetectTargets()
    //{
    //    Collider[] hits = Physics.OverlapSphere(player.position, detectionRange, enemyLayer);
    //    float closestDistance = Mathf.Infinity;
    //    Transform closestTarget = null;

    //    foreach(Collider hit in hits)
    //    {
    //        Vector3 directionToTarget = (hit.transform.position - player.position).normalized;
    //        float angleToTarget = Vector3.Angle(player.forward, directionToTarget);

    //        if(angleToTarget < detectionAngle / 2)
    //        {
    //            float distance = Vector3.Distance(player.position, hit.transform.position);
    //            if (distance < closestDistance)
    //            {
    //                closestDistance = distance;
    //                closestTarget = hit.transform;
    //            }
    //        }

    //    }

    //    currentTarget = closestTarget;
    //}

    //private IEnumerator FaceTargetAndShoot()
    //{
    //    Vector3 direction = (currentTarget.position - player.position).normalized;
    //    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

    //    while(Quaternion.Angle(player.rotation,lookRotation) > 0.1f)
    //    {
    //        player.rotation = Quaternion.Slerp(player.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    //        yield return null;
    //    }

    //    ShootArrow();
    //}

    //private void ShootArrow()
    //{
    //    GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
    //    Rigidbody rb = arrow.GetComponent<Rigidbody>();
    //    rb.velocity = arrowSpawnPoint.forward * 20f;
    //    Debug.Log("Arrow Shot");
    //}

    //public void EnterAimMode()
    //{
    //    if(crosshair != null)
    //    {
    //        crosshair.SetActive(true);
    //    }
    //}

    //public void ExitAimMode()
    //{
    //    if(crosshair != null)
    //    {
    //        crosshair.SetActive(false);
    //    }
    //}
}

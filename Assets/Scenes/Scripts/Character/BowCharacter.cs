using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowCharacter : Character
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed;
    public Camera playerCamera;
    public LayerMask aimLayerMask;

    public CinemachineVirtualCamera thirdPersonCamera;
    public CinemachineVirtualCamera firstPersonCamera;

    private bool isAiming = false;
    private Vector3 aimPoint;

    public override void Attack()
    {
        ShootArrow();
    }

    protected override void Update()
    {
        base.Update();
        if (isActive)
        {
            HandleAiming();
        }

        if (isAiming)
        {
            UpdateAimPoint();
        }
    }

    private void HandleAiming()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (isAiming)
            {
                StopAiming();
            }
            else
            {
                StartAiming();
            }
        }
    }
    private void StartAiming()
    {
        isAiming = true;
        thirdPersonCamera.gameObject.SetActive(false);
        firstPersonCamera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void StopAiming()
    {
        isAiming = false;
        thirdPersonCamera.gameObject.SetActive(true);
        firstPersonCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ShootArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        Vector3 shootingDirection = CalculateShootingDirection();
        rb.velocity = shootingDirection * arrowSpeed;
    }

    private void UpdateAimPoint()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimLayerMask))
        {
            aimPoint = hit.point;
        }
        else
        {
            aimPoint = ray.GetPoint(100);
        }

        Debug.DrawLine(ray.origin, aimPoint, Color.red);
    }

    private Vector3 CalculateShootingDirection()
    {
        return (aimPoint - arrowSpawnPoint.position).normalized;
    }
}

using Cinemachine;
using UnityEngine;

public class Bow : Weapon
{
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    private bool isAiming = false;
    public float aimZoomFOV = 30.0f;
    private float normalFOV;
    public CinemachineVirtualCamera aimCamera;
    private PlayerInputHandler _input;

    private void Start()
    {
        _input = GetComponent<PlayerInputHandler>();
        if(aimCamera != null )
        {
            aimCamera.gameObject.SetActive(false);
            normalFOV = aimCamera.m_Lens.FieldOfView;
        }
    }

    private void Update()
    {
        if (_input.aim) ToggleAiming();
    }

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
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = arrowSpawnPoint.forward * 25.0f;
    }

    private void PerformAimedShot()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = arrowSpawnPoint.forward * 35.0f;
    }

    public void ToggleAiming()
    {
        isAiming = !isAiming;
        if(aimCamera != null)
        {
            aimCamera.m_Lens.FieldOfView = isAiming ? aimZoomFOV : normalFOV;
        }
    }

    
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimModeController : MonoBehaviour
{
    private bool isAimMode = false;
    public CinemachineVirtualCamera aimCamera;
    private PlayerController playerController;
    private CharacterController characterController;
    private PlayerInputHandler playerInputHandler;
    private Animator animator;
    private Cinemachine3rdPersonFollow cinemachineFollowComponent;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        characterController = transform.parent.GetComponent<CharacterController>();
        playerInputHandler = transform.parent.GetComponent<PlayerInputHandler>();
        aimCamera.enabled = false;
        cinemachineFollowComponent = aimCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    public void ToggleAimMode()
    {
        isAimMode = !isAimMode;
        aimCamera.enabled = isAimMode;
        if (isAimMode)
        {
            cinemachineFollowComponent.CameraDistance = 3f;
            aimCamera.m_Lens.FieldOfView = 40f;
        }
        else
        {
            cinemachineFollowComponent.CameraDistance = 6f;
            aimCamera.m_Lens.FieldOfView = 60f;
        }
    }

    public bool IsAimMode()
    {
        return isAimMode;
    }
}

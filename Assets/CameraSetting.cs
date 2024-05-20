using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    private CinemachineVirtualCamera CinemachineVirtualCamera;

    private void Awake()
    {
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        SetCameraFollow();
    }

    public void SetCameraFollow()
    {
        //GameObject[] characterPrefabs = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject character in characterPrefabs)
        {
            if (character.activeInHierarchy)
            {
                Transform cameraLookTransform = character.transform.Find("CameraLook");

                if(cameraLookTransform != null)
                {
                    CinemachineVirtualCamera.Follow = cameraLookTransform;
                    break;
                }
                else
                {
                    Debug.LogWarning("CameraLook 게임 오브젝트를 찾을 수 없습니다.");
                }
            }
        }
    }
}

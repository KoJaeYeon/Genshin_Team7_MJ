using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSetting : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomSpeed = 2f;
    public float minZoom = -2f;
    public float maxZoom = 2f;

    private Cinemachine3rdPersonFollow cinemachine3RdPersonFollow;
    private Vector3 originalShoulderOffset;

    private void Start()
    {
        if(virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            cinemachine3RdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();   
            originalShoulderOffset = cinemachine3RdPersonFollow.ShoulderOffset;
        }
    }

    private void Update()
    {
        float scrollData = Mouse.current.scroll.ReadValue().y;
        if(scrollData != 0)
        {
            Vector3 newOffset = cinemachine3RdPersonFollow.ShoulderOffset;
            newOffset.z += (scrollData * zoomSpeed) / 120f;
            newOffset.z = Mathf.Clamp(newOffset.z, originalShoulderOffset.z + minZoom, originalShoulderOffset.z + maxZoom);

            cinemachine3RdPersonFollow.ShoulderOffset = newOffset;  
        }
    }
}

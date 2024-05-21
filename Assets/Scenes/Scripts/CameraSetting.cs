using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraSetting : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetTarget(Transform newTarget)
    {
        Transform cameraLookTransform = FindChildByName(newTarget, "CameraLook");

        if(cameraLookTransform != null)
        {
            virtualCamera.Follow = cameraLookTransform;
        }
    }

    public void SetCameraSettings(Vector3 position, Quaternion rotation)
    {
        StartCoroutine(SmoothCameraTransition(position, rotation, 0.01f));

        //virtualCamera.transform.position = position;
        //virtualCamera.transform.rotation = rotation;
    }

    private IEnumerator SmoothCameraTransition(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 startPosition = virtualCamera.transform.position;
        Quaternion startRotation = virtualCamera.transform.rotation;
        float elapsed = 0f;

        while(elapsed < duration)
        {
            virtualCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            virtualCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        virtualCamera.transform.position = targetPosition;
        virtualCamera.transform.rotation = targetRotation;
    }

    private Transform FindChildByName(Transform parent, string name)
    {
        foreach(Transform child in parent)
        {
            if(child.name == name)
            {
                return child;
            }
            Transform result = FindChildByName(child, name);
            if(result != null)
            {
                return result;
            }
        }
        return null;
    }


}

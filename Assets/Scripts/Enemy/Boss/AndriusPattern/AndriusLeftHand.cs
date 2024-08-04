using UnityEngine;
public class AndriusLeftHand : MonoBehaviour
{
    private SphereCollider _leftCollider;
    private float _offTime = 1.5f;

    void Start()
    {
        AndriusEventManager.Instance.AddEvent_LeftClawEvent(OnSphereCollider);
        _leftCollider = GetComponent<SphereCollider>();
        _leftCollider.enabled = false;  
    }

    private void OnSphereCollider()
    {
        _leftCollider.enabled = true;
        Invoke(nameof(OffSphereCollider), _offTime);
    }

    private void OffSphereCollider()
    {
        _leftCollider.enabled = false;
    }
}

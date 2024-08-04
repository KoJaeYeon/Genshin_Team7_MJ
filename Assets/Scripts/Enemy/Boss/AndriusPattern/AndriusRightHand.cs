using UnityEngine;

public class AndriusRightHand : MonoBehaviour
{
    private SphereCollider _rightCollider;
    private float _offTime = 1.5f;

    void Start()
    {
        AndriusEventManager.Instance.AddEvent_RightClawEvent(OnSphereCollider);
        _rightCollider = GetComponent<SphereCollider>();
        _rightCollider.enabled = false;
    }

    private void OnSphereCollider()
    {
        _rightCollider.enabled = true;
        Invoke(nameof(OffSphereCollider), _offTime);
    }

    private void OffSphereCollider()
    {
        _rightCollider.enabled = false;
    }
}

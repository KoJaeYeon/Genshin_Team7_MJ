using UnityEngine;
using System.Collections;

public class AndriusRightHand : MonoBehaviour
{
    private SphereCollider _rightCollider;
    private float _offTime = 1.5f;

    void Start()
    {
        _rightCollider = GetComponent<SphereCollider>();
        _rightCollider.enabled = false;

        StartCoroutine(AddInterface());
    }

    private IEnumerator AddInterface()
    {
        yield return new WaitUntil(() => AndriusEventManager.Instance.IsClawInterface);

        AndriusEventManager.Instance.AddEvent_RightClawEvent(OnSphereCollider);
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

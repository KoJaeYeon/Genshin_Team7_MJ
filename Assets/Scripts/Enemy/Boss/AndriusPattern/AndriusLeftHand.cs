using System.Collections;
using UnityEngine;
public class AndriusLeftHand : MonoBehaviour
{
    private SphereCollider _leftCollider;
    private float _offTime = 1.5f;

    void Start()
    {
        _leftCollider = GetComponent<SphereCollider>();
        _leftCollider.enabled = false;

        StartCoroutine(AddInterface());
    }

    private IEnumerator AddInterface()
    {
        yield return new WaitUntil(() => AndriusEventManager.Instance.IsClawInterface);

        AndriusEventManager.Instance.AddEvent_LeftClawEvent(OnSphereCollider);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndriusChargeCollider : MonoBehaviour
{
    private CapsuleCollider _chargeCollider;

    void Start()
    {
        _chargeCollider = GetComponent<CapsuleCollider>();
        _chargeCollider.enabled = false;

        StartCoroutine(AddInterface());
    }

    private IEnumerator AddInterface()
    {
        yield return new WaitUntil(() => AndriusEventManager.Instance.IsChargeInterface);

        AndriusEventManager.Instance.AddEvent_OnChargeColliderEvent(OnCollider);
        AndriusEventManager.Instance.AddEvent_OffColliderEvent(OffCollider);
    }

    private void OnCollider()
    {
        _chargeCollider.enabled = true;
    }

    private void OffCollider()
    {
        _chargeCollider.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndriusChargeCollider : MonoBehaviour
{
    private CapsuleCollider _chargeCollider;

    void Start()
    {
        AndriusEventManager.Instance.AddEvent_OnChargeColliderEvent(OnCollider);
        AndriusEventManager.Instance.AddEvent_OffColliderEvent(OffCollider);

        _chargeCollider = GetComponent<CapsuleCollider>();
        _chargeCollider.enabled = false;
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

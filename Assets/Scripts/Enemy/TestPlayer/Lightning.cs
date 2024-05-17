using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour, IWeapon
{
    private Element element;
    private Vector3 FirePosition;

    private void Awake()
    {
        FirePosition = new Vector3(0, 1, 0);
        element = Element.Lightning;
    }
    public void Shoot()
    {
        GameObject fire = ElementPool.Instance.GetTestElement();
        TestElement test = fire.GetComponent<TestElement>();
        test.SetElement(element);
        Rigidbody rigidbody = fire.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.velocity = new Vector3(0, 0, 10f);
        fire.transform.position = transform.position + FirePosition;
    }

   
}
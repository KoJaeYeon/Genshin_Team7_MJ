using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestElement : MonoBehaviour
{
    private Element element;
    private float Damage = 10f;

    public void SetElement(Element element)
    {
        this.element = element; 
    }

    public Element GetElement()
    {
        return element;
    }

    public float ReturnDamage()
    {
        return Damage;
    }

    public void Return()
    {
        gameObject.SetActive(false);
    }
}

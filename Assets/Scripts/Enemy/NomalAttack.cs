using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAttack : MonoBehaviour
{
    public void OnEnable()
    {
        Collider[] collider = Physics.OverlapBox(transform.position, new Vector3(10f, 10f, 10f), Quaternion.identity);

        foreach(Collider target in collider)
        {
            if (target.gameObject.CompareTag("Player"))
                Debug.Log("플레이어 충돌함");
        }
    }

}

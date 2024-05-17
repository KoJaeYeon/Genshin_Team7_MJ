using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    BoxCollider boxCollider;
    float MonsterAttackPower;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }
    public void EableSword()
    {
        StartCoroutine(UseWeapon());
    }

    public IEnumerator UseWeapon()
    {
        boxCollider.enabled = true;
        yield return new WaitForSeconds(1.0f);
        boxCollider.enabled = false;
    }

    public void SetAttackPower(float attackPower)
    {
        Debug.Log("SetAttackPower");
        MonsterAttackPower = attackPower;
    }

    private void OnTriggerEnter(Collider other) //플레이어에게 피해를 주는 부분
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("충돌함");
            Debug.Log(MonsterAttackPower);
        }
            
    }
}

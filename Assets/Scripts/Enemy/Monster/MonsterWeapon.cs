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

    private void OnTriggerEnter(Collider other) //�÷��̾�� ���ظ� �ִ� �κ�
    {
        if (other.CompareTag("Player"))
        {
            Character player = other.transform.GetComponentInChildren<Character>();
            player.TakeDamage(MonsterAttackPower);
        }
            
    }
}

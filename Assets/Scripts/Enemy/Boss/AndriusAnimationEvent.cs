using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndriusAnimationEvent : MonoBehaviour
{
    private Andrius _andrius;
    private AndriusJumpData _jumpData;
    private AndriusStampData _stampData;
    private AndriusDriftData _driftData;

    private LayerMask _targetLayer;

    void Start()
    {
        GetData();

        _andrius = gameObject.GetComponent<Andrius>();
        _targetLayer = LayerMask.GetMask("Player");
    }

    private void GetData()
    {
        _jumpData = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusJumpData>("AndriusJumpData");
        _stampData = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusStampData>("AndriusStampData");
        _driftData = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusDriftData>("AndriusDriftData");
    }

    public void OnJumpOverlap()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 7f, _targetLayer);

        if(colliders.Length != 0 )
        {
            Character player = colliders[0].gameObject.GetComponentInChildren<Character>(); 

            if(player != null)
            {
                player.TakeDamage(_jumpData.SkillDamage * _andrius.GetAtk());
            }
        }
    }

    public void OnDriftOverlap()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 7f, _targetLayer);

        if (colliders.Length != 0)
        {
            Character player = colliders[0].gameObject.GetComponentInChildren<Character>();

            if (player != null)
            {
                player.TakeDamage(_driftData.SkillDamage * _andrius.GetAtk());
            }
        }
    }

    public void OnStampOverlap()
    {
        Vector3 boxPosition = transform.position +
            transform.TransformDirection(new Vector3(0f, 4f, 15f)) + transform.forward;

        Vector3 boxSize = new Vector3(6f, 8f, 25f);

        Collider[] colliders = Physics.OverlapBox(boxPosition, boxSize / 2, transform.rotation, _targetLayer);

        if (colliders.Length != 0)
        {
            Character player = colliders[0].gameObject.GetComponentInChildren<Character>();

            if (player != null)
            {
                player.TakeDamage(_stampData.SkillDamage * _andrius.GetAtk());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, 7f);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, 10f);

        Gizmos.color = Color.white;

        Vector3 boxSize = new Vector3(6f, 8f, 25f);

        Vector3 boxPosition = transform.position +
            transform.TransformDirection(new Vector3(0f, 4f, 15f)) + transform.forward;

        Gizmos.DrawWireCube(boxPosition, boxSize);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindField : MonoBehaviour
{
    bool trigger = false;
    float radius = 6;
    PlayerInputHandler inputHandler;
    public void Active()
    {
        trigger = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Deactive()
    {
        if(inputHandler != null)
        {
            inputHandler.windfield = false;
        }
        trigger=false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void DestroyField()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (trigger && other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            if (inputHandler == null) inputHandler = other.GetComponent<PlayerInputHandler>();
            inputHandler.windfield = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (trigger && other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            inputHandler.windfield = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
        if (inputHandler != null) inputHandler.windfield=false;
    }

    private void OnDrawGizmos()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + Vector3.up*14;

        Gizmos.DrawWireSphere(startPos,radius);
        Gizmos.DrawWireSphere(endPos, radius);
        Gizmos.DrawLine(startPos + Vector3.right *radius, endPos + Vector3.right *radius);
        Gizmos.DrawLine(startPos + Vector3.left *radius, endPos + Vector3.left *radius);
        Gizmos.DrawLine(startPos + Vector3.forward *radius, endPos + Vector3.forward *radius);
        Gizmos.DrawLine(startPos + Vector3.back *radius, endPos + Vector3.back *radius);
    }
}

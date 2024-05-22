using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindField : MonoBehaviour
{
    bool trigger = false;
    PlayerInputHandler inputHandler;
    public void Active()
    {
        trigger = true;
    }

    public void Deactive()
    {
        if(inputHandler != null)
        {
            inputHandler.windfield = false;
        }
        trigger=false;
    }

    public void DestroyField()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (trigger && other.gameObject.layer.Equals(LayerMask.GetMask("Player")))
        {
            if (inputHandler == null) inputHandler = other.GetComponent<PlayerInputHandler>();
            inputHandler.windfield = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (trigger && other.gameObject.layer.Equals(LayerMask.GetMask("Player")))
        {
            inputHandler.windfield = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inputHandler != null) inputHandler.windfield=false;
    }
}

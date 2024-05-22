using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticle : MonoBehaviour
{
    Mission mission;

    public void SetMission(Mission mission)
    {
        this.mission = mission;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals( LayerMask.NameToLayer("Player")))
        {
            mission.UpdateCount();
            gameObject.SetActive(false);
        }
    }
}

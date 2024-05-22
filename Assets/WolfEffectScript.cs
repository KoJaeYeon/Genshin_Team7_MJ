using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEffectScript : MonoBehaviour
{
    public GameObject spike;
    public Transform forward_Leg;
    public void SpawnSpike()
    {
        spike.transform.position = forward_Leg.position;
        spike.SetActive(false);
        spike.SetActive(true);
        Vector3 vec3 = spike.transform.position;
        vec3.y = 6.32f;
        spike.transform.position = vec3;
    }
}

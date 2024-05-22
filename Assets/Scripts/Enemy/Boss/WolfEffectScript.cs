using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEffectScript : MonoBehaviour
{
    public GameObject spike;
    public GameObject ring;
    public GameObject crystal;
    public GameObject wave;

    public Transform forward_Leg;
    public Transform WavePoint;



    private void Start()
    {
        
    }

    public void CreateObject()
    {
        
    }

    public void SpawnSpike()
    {
        spike.transform.position = forward_Leg.position;
        spike.SetActive(false);
        spike.SetActive(true);
        Vector3 vec3 = spike.transform.position;
        vec3.y = 6.32f;
        spike.transform.position = vec3;
    }

    public void SpawnWave()
    {
        wave.transform.position = WavePoint.position;

        wave.SetActive(false);
        wave.SetActive(true);
    }
}

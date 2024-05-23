using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEffectScript : MonoBehaviour
{
    private enum Effect
    {
        Frost_Wave,
        Frost_Ring,
        Frost_Spike,
        Frost_Crystal
    }

    public GameObject[] effects;
    public Transform effectPool;

    private Dictionary<Effect, GameObject> effectDic;
  

    private void Start()
    {
        
    }

    public void InitObject()
    {
        effectDic.Add(Effect.Frost_Wave, Instantiate(effects[0], effectPool));

       

    }

    private void InitEffectObject()
    {
   
    }
    private void InitEffectTrans()
    {
       
    }
    
    

    public void SpawnSpike()
    {
        //spike.transform.position = forward_Leg.position;
        //spike.SetActive(false);
        //spike.SetActive(true);
        //Vector3 vec3 = spike.transform.position;
        //vec3.y = 6.32f;
        //spike.transform.position = vec3;
    }

    public void SpawnWave()
    {
        //wave.transform.position = WavePoint.position;

        //wave.SetActive(false);
        //wave.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSetting : MonoBehaviour
{
    public GameObject day;
    public GameObject night;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,-Time.deltaTime);
        if(transform.eulerAngles.z < 30)
        {
            if(night.activeSelf)
            {
                night.SetActive(false);
                day.SetActive(true);
            }
        }
        else if (transform.eulerAngles.z < 210)
        {
            if(day.activeSelf)
            {
                day.SetActive(false);
                night.SetActive(true);
            }

        }
    }
}

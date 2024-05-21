using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RanderTextuer : MonoBehaviour
{
    CinemachineFreeLook freeLook;

    private void Start()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
        freeLook.m_YAxis.m_MaxSpeed = 0;
        freeLook.m_XAxis.m_MaxSpeed = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("클릭중");
            freeLook.m_YAxis.m_MaxSpeed = 8;

            freeLook.m_XAxis.m_MaxSpeed = 300;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("클릭땜");
            freeLook.m_YAxis.m_MaxSpeed = 0;
            freeLook.m_XAxis.m_MaxSpeed = 0;
        }
    }


   
}

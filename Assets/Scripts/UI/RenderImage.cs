using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RenderImage : MonoBehaviour
{
    public float speed = 5f;
    Vector3 mousePos;
    Vector3 initMousePos;

    Vector3 initRotaion;

    Vector3 targetRot;

    public bool touched = true;

    private void Start()
    {
        targetRot.z = 0;
    }
    private void Update()
    {
        if (!touched)
        {
            if (transform.eulerAngles.x > 180) { targetRot.x = 1; } else { targetRot.x = 0; }
            if (transform.eulerAngles.y > 180) { targetRot.y = 1; } else { targetRot.y = 0; }            
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRot * 360, Time.deltaTime);

        }

        if(Input.GetKeyDown(KeyCode.I)) touched = true;
        else if(Input.GetKeyDown(KeyCode.K)) touched = false;
    }

    public void OnBeginDrag()
    {
        touched = true;

#if !UNITY_ANDROID
        mousePos = Input.mousePosition;
#else
        mousePos = Input.GetTouch(0).position;
#endif
        initMousePos = mousePos;

        initRotaion = transform.rotation.eulerAngles;
        if (initRotaion.x <= 360 && initRotaion.x > 20) initRotaion.x -= 360;
    }
    public void OnPointDrag()
    {
#if !UNITY_ANDROID
        mousePos = Input.mousePosition;
#else
        mousePos = Input.GetTouch(0).position;
#endif
        Vector3 difVec3 = mousePos - initMousePos;

        Vector3 vector3;
        vector3.x = initRotaion.x + difVec3.y * speed;
        
        Debug.Log(vector3.x);
        vector3.x = Mathf.Clamp(vector3.x , -60f, 20f);

        // 차이 구해서 해당 벡터로 카메라 회전시키기
        transform.eulerAngles = new Vector3(vector3.x , initRotaion.y + difVec3.x * speed, 0);        
    }



    public void OnEndDrag()
    {
#if !UNITY_ANDROID
        mousePos = Input.mousePosition;
#else
        mousePos = Input.GetTouch(0).position;
#endif
        initMousePos = mousePos;

        initRotaion = transform.rotation.eulerAngles;
        if (initRotaion.x <= 360 && initRotaion.x > 20) initRotaion.x -= 360;
    }
}

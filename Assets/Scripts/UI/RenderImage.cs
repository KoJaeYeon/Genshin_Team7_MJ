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


    public void OnPointDrag()
    {
        mousePos = Input.mousePosition;

        Vector3 difVec3 = mousePos - initMousePos;

        Vector3 vector3;
        vector3.x = initRotaion.x + difVec3.y * speed;
        
        Debug.Log(vector3.x);
        vector3.x = Mathf.Clamp(vector3.x , -60f, 20f);

        // 차이 구해서 해당 벡터로 카메라 회전시키기
        transform.eulerAngles = new Vector3(vector3.x , initRotaion.y + difVec3.x * speed, 0);
        
    }

    public void OnBeginDrag()
    {
        mousePos = Input.mousePosition;

        initMousePos = mousePos;

        initRotaion = transform.rotation.eulerAngles;
        if (initRotaion.x <= 360 && initRotaion.x > 20) initRotaion.x -= 360;
    }

    public void OnEndDrag()
    {
        mousePos = Input.mousePosition;

        initMousePos = mousePos;

        initRotaion = transform.rotation.eulerAngles;
        if (initRotaion.x <= 360 && initRotaion.x > 20) initRotaion.x -= 360;
    }






}

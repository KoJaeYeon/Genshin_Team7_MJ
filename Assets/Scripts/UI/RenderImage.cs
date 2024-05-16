using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RenderImage : MonoBehaviour
{
    public float mousespeed = 200f; //마우스감도

    private float MouseY;
    private float MouseX;

    
    private void Rotate()
    {
        
        

            MouseX += Input.GetAxisRaw("Mouse X") * mousespeed * Time.deltaTime;

            MouseY -= Input.GetAxisRaw("Mouse Y") * -mousespeed * Time.deltaTime;

            MouseY = Mathf.Clamp(MouseY, -70f, 20f); //Clamp를 통해 최소값 최대값을 넘지 않도록함

            transform.localRotation = Quaternion.Euler(MouseY, MouseX, 0f);// 각 축을 한꺼번에 계산
        
    }
    
    /*
    public GameObject camera;

    public float speed = 5f;

    void Update()
    {

       Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
       Input.mousePosition.y, -Camera.main.transform.position.z));

       Debug.Log(mousePos.ToString());
        
    }
    public void OnPointDrag()
    {
        Debug.Log("스크롤");
        camera.transform.Rotate(new Vector3(1 , 0 , 0) * speed);//X좌표 움직임
    
    }
    */





}

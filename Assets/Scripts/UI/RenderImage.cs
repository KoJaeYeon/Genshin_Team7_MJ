using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RenderImage : MonoBehaviour
{
    public float mousespeed = 200f; //���콺����

    private float MouseY;
    private float MouseX;

    
    private void Rotate()
    {
        
        

            MouseX += Input.GetAxisRaw("Mouse X") * mousespeed * Time.deltaTime;

            MouseY -= Input.GetAxisRaw("Mouse Y") * -mousespeed * Time.deltaTime;

            MouseY = Mathf.Clamp(MouseY, -70f, 20f); //Clamp�� ���� �ּҰ� �ִ밪�� ���� �ʵ�����

            transform.localRotation = Quaternion.Euler(MouseY, MouseX, 0f);// �� ���� �Ѳ����� ���
        
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
        Debug.Log("��ũ��");
        camera.transform.Rotate(new Vector3(1 , 0 , 0) * speed);//X��ǥ ������
    
    }
    */





}

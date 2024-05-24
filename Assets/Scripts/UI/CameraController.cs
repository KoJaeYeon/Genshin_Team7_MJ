using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;


public class CameraController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public PlayerInputHandler playerInputHandler;

    //ī�޶� ������
    public float cameraSensitvityz = 50;//ī�޶� ȸ�� �ΰ��� 
    public float perspectiveZoomSpeed = 5f; //perspective : �� �ӵ� 

    PointerEventData[] eventDatas;
    int touchCount = 0;

    private void Awake()
    {
#if !UNITY_ANDROID
        //Destroy(this.gameobject);
#endif
        eventDatas = new PointerEventData[20];
    }
    private void Update()
    {
        GetTouchInput();
    }


    private void GetTouchInput()
    {
        for (int i = 0; i < touchCount; i++)
        {
            PointerEventData t = eventDatas[i];

            if (touchCount == 1)
            {
                Vector2 input_Look = t.delta;
                input_Look.y *= -1;
                playerInputHandler.look = input_Look * cameraSensitvityz * Time.deltaTime;
            }
            else if (touchCount == 2)
            {
                PointerEventData touchZero = eventDatas[0];
                PointerEventData touchOne = eventDatas[1];

                //ó�� ��ġ�� ��ġ - ���� ��ġ�� ��ġ 
                Vector2 touchZeroMove = touchZero.position - touchZero.delta;
                Vector2 touchOneMove = touchOne.position - touchOne.delta;

                //magnitude : �Ÿ� ���̸� �����ִ� ���� ��
                float touchDaltaMag = (touchZeroMove - touchOneMove).magnitude;//���� �������� �� ��ġ ������ �Ÿ�

                float touchMag = (touchZero.position - touchOne.position).magnitude;//���� �������� �� ��ġ ������ �Ÿ�

                //������ �Ÿ����� ���� -> ���̳ʽ��� ������ �հ����� ������ �ִ°�
                float touchZoom = touchDaltaMag - touchMag;
                float minus = touchZoom < 0 ? 1 : -1;

                touchZoom = t.delta.magnitude;

                playerInputHandler.zoom += touchZoom * perspectiveZoomSpeed * Time.deltaTime * minus;
            }


        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        eventDatas[touchCount] = eventData;
        touchCount++;
        Debug.Log("down");

    }

    public void OnPointerMove(PointerEventData eventData)
    {
        int index = 0;
       
        foreach (PointerEventData pointerEventData in eventDatas)
        {
            if (pointerEventData.pointerId == eventData.pointerId)
            {
           
                eventDatas[index] = eventData;
            }
            index++;

            
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        touchCount--;
    }
}


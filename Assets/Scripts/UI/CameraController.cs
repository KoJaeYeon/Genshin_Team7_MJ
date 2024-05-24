using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;


public class CameraController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public PlayerInputHandler playerInputHandler;

    //카메라 움직임
    public float cameraSensitvityz = 50;//카메라 회전 민감도 
    public float perspectiveZoomSpeed = 5f; //perspective : 줌 속도 

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

                //처음 터치한 위치 - 현재 터치된 위치 
                Vector2 touchZeroMove = touchZero.position - touchZero.delta;
                Vector2 touchOneMove = touchOne.position - touchOne.delta;

                //magnitude : 거리 사이를 비교해주는 백터 값
                float touchDaltaMag = (touchZeroMove - touchOneMove).magnitude;//이전 프레임의 두 터치 사이의 거리

                float touchMag = (touchZero.position - touchOne.position).magnitude;//현재 프레임의 두 터치 사이의 거리

                //움직인 거리사이 구함 -> 마이너스가 나오면 손가락을 벌리고 있는것
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


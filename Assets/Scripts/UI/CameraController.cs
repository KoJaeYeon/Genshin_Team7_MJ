using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class CameraController : MonoBehaviour
{
    private PlayerInputHandler playerInputHandler; 

    //카메라 움직임
    public float cameraSensitvity;//카메라 회전 민감도 
    public float perspectiveZoomSpeed = 0.5f; //perspective : 줌 속도 

    //공간 분리
    int leftFingerId, rightFingerId;

    float halfScreenWidth, halfScreenHeight;
  

    private void Awake()
    {
        #if !UNITY_ANDROID
        Destroy(this);
        #endif
        playerInputHandler = GetComponent<PlayerInputHandler>();
    }


    private void Start()
    {
        leftFingerId = -1;
        rightFingerId = -1;
        halfScreenWidth = Screen.width*2 / 5;
        halfScreenHeight = Screen.height / 3;

    }

    private void Update()
    {
        GetTouchInput();
    }


    private void GetTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);

            switch (t.phase)
            {
                //처음화면에 닿는 순간
                case TouchPhase.Began:

                    if (t.position.x < halfScreenWidth && t.position.y < halfScreenHeight && leftFingerId == -1)
                    {
                        leftFingerId = t.fingerId;
                        Debug.Log("왼쪽아래");

                    }
                    else if (t.position.x > halfScreenWidth * 3 / 2 && t.position.y < halfScreenHeight && leftFingerId == -1)
                    {
                        leftFingerId = t.fingerId;
                        Debug.Log("오른쪽아래");

                    }
                    else if (t.position.x > halfScreenWidth && rightFingerId == -1)
                    {
                           rightFingerId = t.fingerId;
                           Debug.Log("오른쪽");
                       
                    }
                    else if (t.position.y > halfScreenHeight && rightFingerId == -1)
                    {
                        rightFingerId = t.fingerId;
                        Debug.Log("왼쪽위");

                    }
                break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (t.fingerId == leftFingerId)
                    {
                        leftFingerId = -1;
                        Debug.Log("왼쪽을 땜");
                    }
                    else if (t.fingerId == rightFingerId)
                    {
                        rightFingerId = -1;
                        Debug.Log("오른쪽을 땜");
                        playerInputHandler.look = Vector2.zero;
                    }
                break;
                   
                    //움직일때
                case TouchPhase.Moved:

                    if (t.fingerId == rightFingerId)
                    {
                        //카메라를 부드럽게 만들기
                        Vector2 input_Look = t.deltaPosition;
                        input_Look.y *= -1;
                        playerInputHandler.look = input_Look *cameraSensitvity * Time.deltaTime;
                        
                    }

                    if (t.fingerId == rightFingerId && Input.touchCount > 1 && leftFingerId == -1)
                    {

                        Touch touchZero = Input.GetTouch(0);
                        Touch touchOne = Input.GetTouch(1);

                        //처음 터치한 위치 - 현재 터치된 위치 
                        Vector2 touchZeroMove = touchZero.position - touchZero.deltaPosition;
                        Vector2 touchOneMove = touchOne.position - touchOne.deltaPosition;

                        //magnitude : 거리 사이를 비교해주는 백터 값
                        float touchDaltaMag = (touchZeroMove - touchOneMove).magnitude;//이전 프레임의 두 터치 사이의 거리

                        float touchMag = (touchZero.position - touchOne.position).magnitude;//현재 프레임의 두 터치 사이의 거리

                        //움직인 거리사이 구함 -> 마이너스가 나오면 손가락을 벌리고 있는것
                        float touchZoom = touchDaltaMag - touchMag;
                        float minus = touchZoom < 0 ? 1 : -1;
                        
                        touchZoom = t.deltaPosition.magnitude;

                        playerInputHandler.zoom += touchZoom * perspectiveZoomSpeed * Time.deltaTime * minus; 

                        //Debug.Log("touchZoom :" + touchZoom);
                      
                    }

                break;
               


            }



        }
    }




}


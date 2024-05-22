using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class CameraController : MonoBehaviour
{
    private PlayerInputHandler playerInputHandler; 


    //ī�޶� ������
    public float cameraSensitvity;//ī�޶� ȸ�� �ΰ��� 

    Vector2 lookInput;
    float cameraPitch;//ȸ��


    //���� �и�
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
                //ó��ȭ�鿡 ��� ����
                case TouchPhase.Began:

                    if (t.position.x < halfScreenWidth && t.position.y < halfScreenHeight && leftFingerId == -1)
                    {
                        leftFingerId = t.fingerId;
                        Debug.Log("���ʾƷ�");

                    }
                    else if (t.position.x > halfScreenWidth * 3 / 2 && t.position.y < halfScreenHeight && leftFingerId == -1)
                    {
                        leftFingerId = t.fingerId;
                        Debug.Log("�����ʾƷ�");
                    }
                    else if (t.position.x > halfScreenWidth && rightFingerId == -1)
                    {

                        if (Input.touchCount == 2)
                        {
                            Touch touchZero = Input.GetTouch(0);
                            Touch touchOne = Input.GetTouch(1);

                            //ó�� ��ġ�� ��ġ - ���� ��ġ�� ��ġ 
                            Vector2 touchZeroMove = touchZero.position - touchOne.deltaPosition;
                            Vector2 touchOneMove = touchOne.position - touchOne.deltaPosition;

                            //magnitude : �Ÿ� ���̸� �����ִ� ���� ��
                            float touchDaltaMag = ( touchZeroMove - touchOneMove).magnitude;
                            float touchMag = (touchZero.position - touchOne.position).magnitude;

                            //�Ÿ����� ���� -> ���̳ʽ��� ������ �հ����� ������ �ִ°�
                            float touchZoom = touchDaltaMag - touchMag;

                            Debug.Log("touchZoom :" + touchZoom);



                        }
                        else 
                        {
                           rightFingerId = t.fingerId;
                           Debug.Log("������");
                        }
                        

                    }
                    else if (t.position.y > halfScreenHeight && rightFingerId == -1)
                    {
                        rightFingerId = t.fingerId;
                        Debug.Log("������");

                    }
                break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (t.fingerId == leftFingerId)
                    {
                        leftFingerId = -1;
                        Debug.Log("������ ��");
                    }
                    else if (t.fingerId == rightFingerId)
                    {
                        rightFingerId = -1;
                        Debug.Log("�������� ��");
                        playerInputHandler.look = Vector2.zero;
                    }
                break;
                    //ī�޶�
                    //�����϶�
                case TouchPhase.Moved:
                    if (t.fingerId == rightFingerId)
                    {
                        //ī�޶� �ε巴�� �����
                        Vector2 input_Look = t.deltaPosition;
                        input_Look.y *= -1;
                        playerInputHandler.look = input_Look *cameraSensitvity * Time.deltaTime;
                        Debug.Log(t.deltaPosition);
                    }
                break;
                    //�����Ǿ� ������
                case TouchPhase.Stationary:
                    if (t.fingerId == rightFingerId)
                    {
                        //ȸ�� �ʱ�ȭ 
                        lookInput = Vector2.zero;
                    }
                break;


            }



        }
    }




}


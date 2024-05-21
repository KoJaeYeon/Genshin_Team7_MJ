using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMove : MonoBehaviour , IBeginDragHandler , IDragHandler , IEndDragHandler
{
    //�������� �������� ũ��� ȸ���� �����ؾ���
    //ȸ�� : 


    public  RectTransform joy; //������ ���̽�ƽ
    private  RectTransform pointJoy; //�߽��� �� ����


    private void Awake()
    {
        pointJoy = GetComponent<RectTransform>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin : "+ eventData.position);

        

    }


    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag press : " + eventData.pressPosition);
        Debug.Log("Drag pos : " + eventData.position);

       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End");

     

    }



}

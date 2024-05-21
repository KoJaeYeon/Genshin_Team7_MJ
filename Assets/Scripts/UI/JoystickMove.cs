
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMove : MonoBehaviour  , IDragHandler , IEndDragHandler
{
    //�������� �������� ũ��� ȸ���� �����ؾ���
    //ȸ�� : 


    public  RectTransform joy; //������ ���̽�ƽ
    private RectTransform pointJoy; //�߽��� �� ����

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag press : " + eventData.pressPosition);
        Debug.Log("Drag pos : " + eventData.position);

        //pressPosition :���콺�� ������ġ/
        // ���� ��ġ���� �� ��ŭ joy.rotaȸ��
        //���콺�� ��ġ�� ����

        var Rot = eventData.position - eventData.pressPosition; 
        Debug.Log("Rot :" + Rot.normalized);

   
            
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End");

        
        
    }



}

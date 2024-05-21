
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMove : MonoBehaviour  , IDragHandler , IEndDragHandler
{
    //�������� �������� ũ��� ȸ���� �����ؾ���
    //ȸ�� : 


    public  RectTransform joy; //������ ���̽�ƽ
    public  RectTransform pointJoy; //�߽��� �� ����


    private void Start()
    {
        Debug.Log(pointJoy.position);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag press : " + eventData.pressPosition);
        Debug.Log("Drag pos : " + eventData.position);

        //pressPosition :���콺�� ������ġ/
        // ���� ��ġ���� �� ��ŭ joy.rotaȸ��
        //���콺�� ��ġ�� ����

        var Rot = eventData.position - (Vector2)pointJoy.position;
        Rot = Rot.normalized;
        Debug.Log("Rot :" + Rot);
        
        //��ġ ���󰡱� 

        //���� �����°�
        float rad = math.acos(Rot.x) / math.PI * 180;
        if (Rot.y < 0) rad = 360 - rad;
        joy.eulerAngles = new Vector3(0, 0,rad);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End");
       
       joy.rotation = Quaternion.identity;
        
    }



}

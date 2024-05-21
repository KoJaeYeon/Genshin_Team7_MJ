
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMove : MonoBehaviour  , IDragHandler , IEndDragHandler
{
    //기준점을 기준으로 크기와 회전을 조정해야함
    //회전 : 


    public  RectTransform joy; //움직일 조이스틱
    public  RectTransform pointJoy; //중심이 될 조이


    private void Start()
    {
        Debug.Log(pointJoy.position);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag press : " + eventData.pressPosition);
        Debug.Log("Drag pos : " + eventData.position);

        //pressPosition :마우스의 시작위치/
        // 현제 위치값을 뺀 만큼 joy.rota회전
        //마우스의 위치를 따라감

        var Rot = eventData.position - (Vector2)pointJoy.position;
        Rot = Rot.normalized;
        Debug.Log("Rot :" + Rot);
        
        //위치 따라가기 

        //각도 돌리는거
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

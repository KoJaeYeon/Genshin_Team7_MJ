using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMove : MonoBehaviour , IBeginDragHandler , IDragHandler , IEndDragHandler
{
    //기준점을 기준으로 크기와 회전을 조정해야함
    //회전 : 


    public  RectTransform joy; //움직일 조이스틱
    private  RectTransform pointJoy; //중심이 될 조이


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerMove : MonoBehaviour
{
    Vector2 inputvec;
    public float speed = 10f;
    Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
     
    public void Update()
    {
        Vector3 nextvec = inputvec * speed * Time.deltaTime;
        rigid.MovePosition(transform.position + nextvec);
    }

    public void OnTestMove(InputValue value)
    {
        inputvec = value.Get<Vector2>();
    }
}

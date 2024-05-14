using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementObject : MonoBehaviour
{
    private Rigidbody ObjectRigidbody;
    public Transform Player;
    private bool targetMove = false;
    public float Speed;
    private Vector3 targetPos;
    private void Awake()
    {
        ObjectRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(UP());
    }

    private void Update()
    {
        if (targetMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.position, Speed * Time.deltaTime);

            //targetPos = (Player.position - transform.position).normalized;
            //Vector3 Move = targetPos * Speed * Time.deltaTime;

            //transform.Translate(Move);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            gameObject.SetActive(false);
    }

    private IEnumerator UP()
    {
        float Timer = 0f;

        float randx = Random.Range(0.5f, 1);
        float randz = Random.Range(0.5f, 1);

        while (true && Timer < 0.6f)
        {
            ObjectRigidbody.AddForce(new Vector3(randx,1,randz));
            Timer += Time.deltaTime;
        }

        yield return new WaitForSeconds(0.7f);
        targetMove = true;

        ObjectRigidbody.velocity = Vector3.zero;
        ObjectRigidbody.useGravity = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementObject : MonoBehaviour
{
    public Transform Player;
    private Rigidbody ObjectRigidbody;
    private float Speed;
    private bool targetMove = false;
    
    private void Awake()
    {
        ObjectRigidbody = GetComponent<Rigidbody>();
    }

    //private void OnEnable()
    //{
    //    StartCoroutine(UP());
    //}

    private void Update()
    {
        if (targetMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.position, Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            RetrunObject();
    }

    //public void SetPlayerTransform(Transform Player)
    //{
    //    this.Player = Player;
    //}

    public void RetrunObject()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator UP()
    {
        Debug.Log("»£√‚");
        float Timer = 0f;

        float randx = Random.Range(-0.5f, 0.5f);
        float randz = Random.Range(-0.5f, 0.5f);

        while (true && Timer < 0.5f)
        {
            ObjectRigidbody.AddForce(new Vector3(randx,1,randz));
            Timer += Time.deltaTime;
        }

        yield return new WaitForSeconds(1f);
        targetMove = true;

        ObjectRigidbody.velocity = Vector3.zero;
        ObjectRigidbody.useGravity = false;
    }

}

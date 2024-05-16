using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementObject : MonoBehaviour
{
    public Transform Player;
    private Rigidbody ObjectRigidbody;
    private float Speed = 5f;
    private float ElementGauge = 10f;
    private float Power = 400.0f;
    private bool targetMove = false;
    
    private void Awake()
    {
        ObjectRigidbody = GetComponent<Rigidbody>();
    }

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
            gameObject.SetActive(false);            
    }

    public void SetPlayerTransform(Transform Player)
    {
        this.Player = Player;
    }

    public float FillElement()
    {
        return ElementGauge;
    }

    public IEnumerator UP()
    {
        float randx = Random.Range(-0.5f, 0.5f);
        float randz = Random.Range(-0.5f, 0.5f);
        
        ObjectRigidbody.AddForce(new Vector3(randx, 1, randz) * Power);
         
        yield return new WaitForSeconds(0.7f);
        targetMove = true;

        ObjectRigidbody.velocity = Vector3.zero;
        ObjectRigidbody.useGravity = false;
    }

}

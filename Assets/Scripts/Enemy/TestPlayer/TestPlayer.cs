using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public float MoveSpeed;
    public float RotSpeed;
    public float Jumpforce;
    private bool isGround;
    private IWeapon weapon;
    private Element element;
    public int Atk = 20;

    public LayerMask layer;
    Vector3 Movedir = Vector3.zero;

    Rigidbody m_PlayerRigidbody;

    void Start()
    {
        m_PlayerRigidbody = GetComponent<Rigidbody>();
        SetWeapon(Element.Fire);
    }

    private void Update()
    {
        Movedir.z = Input.GetAxis("Vertical");
        Movedir.x = Input.GetAxis("Horizontal");

        Movedir.Normalize();
        CheckGround();

        

        //if (Input.GetButtonDown("Jump") && isGround)
        //{
        //    Vector3 JumpPower = Vector3.up * Jumpforce;
        //    m_PlayerRigidbody.AddForce(JumpPower, ForceMode.VelocityChange);
        //}

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("´­¸²");
            weapon.Shoot();
        }
        else if (Input.GetKeyDown(KeyCode.F1))
            SetWeapon(Element.Fire);
        else if (Input.GetKeyDown(KeyCode.F2))
            SetWeapon(Element.Ice);
        else if (Input.GetKeyDown(KeyCode.F3))
            SetWeapon(Element.Lightning);
        else if(Input.GetKeyDown(KeyCode.F4))
            SetWeapon(Element.Nomal);

    }

    private void FixedUpdate()
    {
        if (Movedir != Vector3.zero)
        {
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(Movedir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(Movedir.z))
            {
                transform.Rotate(Vector3.up);
            }
            transform.forward = Vector3.Lerp(transform.forward, Movedir, RotSpeed * Time.deltaTime);
        }

        m_PlayerRigidbody.MovePosition(gameObject.transform.position + Movedir * MoveSpeed * Time.fixedDeltaTime);

    }

    private void CheckGround()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position + Vector3.up * 0.2f, Vector3.down, Color.black, 0.4f);

        if (Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, out hit, 0.4f, layer))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }

    public void SetWeapon(Element element)
    {
        this.element = element;

        Component component = gameObject.GetComponent<IWeapon>() as Component;

        if (component != null)
        {
            Destroy(component);
        }

        switch (element)
        {
            case Element.Fire:
                weapon = gameObject.AddComponent<Fire>();
                break;
            case Element.Ice:
                weapon = gameObject.AddComponent<Ice>();
                break;
            case Element.Lightning:
                weapon = gameObject.AddComponent<Lightning>();
                break;
            case Element.Nomal:
                weapon = gameObject.AddComponent<Nomal>();
                break;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Debug.Log(enemy);
            enemy.Damaged(enemy, Atk);
            //Debug.Log(enemy.hilichurlHealthDic[enemy]);

        }

    }

}

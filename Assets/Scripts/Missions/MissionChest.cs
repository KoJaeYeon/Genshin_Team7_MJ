using System.Collections;
using UnityEngine;

public class MissionChest : MonoBehaviour,IInteractable
{
    DropObject[] dropObjects;
    GetSlot getSlot;
    static int idtemp = 1;
    public int[] keys;
    public InteractableType chestType;
    int id;

    public GameObject[] chuchus;
    BoxCollider boxCollider;
    GameObject particle;
    GameObject particle2;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        dropObjects = new DropObject[keys.Length];
        particle = transform.GetChild(1).gameObject;
        particle2 = transform.GetChild(2).gameObject;

        getSlot = PoolManager.Instance.Get_GetSlot();
        for (int i = 0;  i < keys.Length; i++)
        {
            dropObjects[i] = PoolManager.Instance.Get_DropObject(keys[i]);
            dropObjects[i].gameObject.SetActive(false);
        }
        InitItemSlot();
        StartCoroutine(Check());
    }

    IEnumerator Check()
    {
        while (true)
        {
            int index = 0;
            foreach(GameObject chuchu in chuchus)
            {
                if (!chuchu.activeSelf) index++;
                else break;
            }
            if(index == chuchus.Length)
            {
                particle.SetActive(true);
                particle2.SetActive(false);
                boxCollider.enabled = true;
                yield break;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void InitItemSlot()
    {
        getSlot.Init(chestType);
    }

    public void SetId(int id)
    {
        this.id = id;
    }
    public void UpdateItemGet()
    {
        Debug.Log("UpdateItemGet_Chest");
        UIManager.Instance.AddGetSlot(getSlot);
        getSlot.gameObject.SetActive(true);
    }

    public void UseItemGet()
    {
        Debug.Log("UseItem_Chest");
        PoolManager.Instance.Return_GetSlot(getSlot);
        foreach(DropObject dropObject in dropObjects)
        {
            dropObject.gameObject.SetActive(true);
            dropObject.transform.position = transform.position + new Vector3(Random.Range(0.1f,0.3f),0,Random.Range(0.1f,0.3f));
        }
        StartCoroutine(DisappearChest());
    }

    IEnumerator DisappearChest()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("Chest_Open");
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        yield return new WaitForSeconds(2f);
        collider.enabled = true;
        gameObject.SetActive(false);

    }
    public void RemoveItemGet()
    {
        Debug.Log("RemoveGet_Chest");
        UIManager.Instance.RemoveGetSlot();
        getSlot.transform.SetParent(PoolManager.Instance.PoolParent);
        getSlot.gameObject.SetActive(false);
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        if (!(obj is MissionChest)) return false;
        MissionChest other = (MissionChest)obj;
        if(other.id == id) return true;
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

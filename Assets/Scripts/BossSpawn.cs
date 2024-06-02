using System.Collections;
using UnityEngine;

public class BossSpawn : MonoBehaviour, IInteractable
{
    GetSlot getSlot;
    public InteractableType interactableType;
    int id;
    bool missionStart = false;
    public GameObject cineMachine;

    public Animator animator;
    SphereCollider sphereCollider;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    private void Start()
    {
        getSlot = PoolManager.Instance.Get_GetSlot();
        InitGetSlot();
    }

    public void InitGetSlot()
    {
        getSlot.Init(interactableType);
    }

    public void SetId(int id)
    {
        this.id = id;
    }
    public void UpdateItemGet()
    {
        Debug.Log("UpdateItemGet_Mission");
        UIManager.Instance.AddGetSlot(getSlot);
        getSlot.gameObject.SetActive(true);
    }

    public void UseItemGet()
    {
        if (MissionManager.Instance.mission == null) // 미션시작
        {
            Debug.Log("UseItem_Mission"); // Init Mission
            sphereCollider.enabled = false;
            missionStart = true;

            getSlot.transform.SetParent(PoolManager.Instance.PoolParent);
            getSlot.gameObject.SetActive(false);
            cineMachine.SetActive(true);
            gameObject.SetActive(false);
            
            SoundManager.Instance.ChangeBGM(SoundManager.Instance.bossBGM);
        }
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
        if (!(obj is BossSpawn)) return false;
        BossSpawn other = (BossSpawn)obj;
        if (other.id == id) return true;
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

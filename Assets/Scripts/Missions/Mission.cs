using System.Collections;
using UnityEngine;

public class Mission : MonoBehaviour, IInteractable
{
    GetSlot getSlot;
    public GameObject[] particles;
    public InteractableType interactableType;
    int id;
    public float maxTime;
    float nowTime;
    int previousTime;
    int count;
    bool missionStart = false;

    SphereCollider sphereCollider;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        particles = new GameObject[transform.GetChild(0).childCount];
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i] = transform.GetChild(0).GetChild(i).gameObject;
            particles[i].GetComponent<WindParticle>().SetMission(this);
        }
    }
    private void Start()
    {
        getSlot = PoolManager.Instance.Get_GetSlot();
        InitGetSlot();
    }

    private void Update()
    {
        if(missionStart)
        {
            nowTime -= Time.deltaTime;
            if ((int)nowTime != previousTime)
            {
                previousTime = (int)nowTime;
                MissionManager.Instance.UpdateTime(previousTime);
            }
            if (nowTime < 0)
            {
                sphereCollider.enabled = true;
                missionStart = false;
                foreach(var particle in particles)
                {
                    particle.gameObject.SetActive(false);
                }
                MissionManager.Instance.Failed();
            }
        }
    }

    public void UpdateCount()
    {
        count++;
        MissionManager.Instance.UpdateObj(count);
        if(count == particles.Length)
        {
            MissionManager.Instance.Succeed();
            missionStart = false;
            PoolManager.Instance.Return_GetSlot(getSlot);
            transform.GetChild(1).gameObject.SetActive(true);
        }
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
        if(MissionManager.Instance.mission == null)
        {
            Debug.Log("UseItem_Mission"); // Init Mission
            sphereCollider.enabled = false;
            nowTime = maxTime;
            previousTime = (int)maxTime;
            count = 0;
            missionStart = true;
            MissionManager.Instance.StartMission(previousTime, particles.Length);
            MissionManager.Instance.mission = this;
            foreach(var item in particles)
            {
                item.SetActive(true);
            }

            getSlot.transform.SetParent(PoolManager.Instance.PoolParent);
            getSlot.gameObject.SetActive(false);
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
        if (!(obj is Mission)) return false;
        Mission other = (Mission)obj;
        if (other.id == id) return true;
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

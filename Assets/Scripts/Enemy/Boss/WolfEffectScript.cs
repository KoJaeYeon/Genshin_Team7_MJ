using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class WolfEffectScript : MonoBehaviour
{
    public enum Effect
    {
        Frost_Wave,
        Frost_Ring,
        Frost_Spike,
        Frost_Crystal,
        Magic,
        Tail_Circle,
        Rain
    }

    private Wolf wolf;
    private Jump jumpSkill;
    private Stamp stampSkill;
    private Howl howlSkill;
    private Drift driftSkill;

    public GameObject Frost_Wave;
    public GameObject Frost_Ring;
    public GameObject Frost_Spike;
    public GameObject Frost_Crystal;
    public GameObject Magic_Circle;
    public GameObject Tail_Circle;
    public GameObject Ice_Rain;

    public Transform WavePoint;
    public Transform RingPoint;
    public Transform SpikePoint;
    public Transform CrystalPoint;
    public Transform MagicPoint;
    public Transform TailPoint;
    public Transform IcePoint;

    private Dictionary<Effect, GameObject> EffectDic;
    private Dictionary<Effect, Transform> EffectTrans;

    public Transform EffectPool;
    
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        wolf = GetComponent<Wolf>();
        EffectDic = new Dictionary<Effect, GameObject>();
        EffectTrans = new Dictionary<Effect, Transform>();

        InstantiateFrost_Wave();
        InstantiateFrost_Ring();
        InstantiateFrost_Spike();
        InstantiateFrost_Crystal();
        InstantiateMagic();
        InstantiateTail_Circle();
        InstantiateIce_Rain();
    }

    private void InstantiateFrost_Wave()
    {
        GameObject frost_Wave = Instantiate(Frost_Wave, WavePoint);
        stampSkill = frost_Wave.GetComponent<Stamp>();
        frost_Wave.SetActive(false);
        EffectDic.Add(Effect.Frost_Wave, frost_Wave);
        EffectTrans.Add(Effect.Frost_Wave, WavePoint);
        frost_Wave.transform.localPosition = new Vector3(0, 0, 12f);
        Vector3 parentforward = GetEffectTransform(Effect.Frost_Wave).forward;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, parentforward);
        frost_Wave.transform.rotation = rotation;
    }

    private void InstantiateFrost_Ring()
    {
        GameObject frost_Ring = Instantiate(Frost_Ring, RingPoint);
        jumpSkill = frost_Ring.GetComponent<Jump>();
        frost_Ring.SetActive(false);
        EffectDic.Add(Effect.Frost_Ring, frost_Ring);
        EffectTrans.Add(Effect.Frost_Ring, RingPoint);
        frost_Ring.transform.localPosition = Vector3.zero;
    }

    private void InstantiateFrost_Spike()
    {
        GameObject frost_Spike = Instantiate(Frost_Spike, SpikePoint);
        frost_Spike.SetActive(false);
        EffectDic.Add(Effect.Frost_Spike, frost_Spike);
        EffectTrans.Add(Effect.Frost_Spike, SpikePoint);
        frost_Spike.transform.localPosition = Vector3.zero;
    }

    private void InstantiateFrost_Crystal()
    {
        GameObject frost_Crystal = Instantiate(Frost_Crystal, CrystalPoint);
        frost_Crystal.SetActive(false);
        EffectDic.Add(Effect.Frost_Crystal, frost_Crystal);
        EffectTrans.Add(Effect.Frost_Crystal, CrystalPoint);
        frost_Crystal.transform.localPosition = Vector3.zero;
    }

    private void InstantiateMagic()
    {
        GameObject Magic = Instantiate(Magic_Circle, MagicPoint);
        howlSkill = Magic.GetComponent<Howl>();
        Magic.SetActive(false);
        EffectDic.Add(Effect.Magic, Magic);
        EffectTrans.Add(Effect.Magic, MagicPoint);
        Magic.transform.localPosition = Vector3.zero;
    }

    private void InstantiateTail_Circle()
    {
        GameObject tail_circle = Instantiate(Tail_Circle, TailPoint);
        driftSkill = tail_circle.GetComponent<Drift>();
        tail_circle.SetActive(false);
        EffectDic.Add(Effect.Tail_Circle, tail_circle);
        EffectTrans.Add(Effect.Tail_Circle, TailPoint);
        tail_circle.transform.localPosition = Vector3.zero;
    }

    private void InstantiateIce_Rain()
    {
        GameObject Ice = Instantiate(Ice_Rain, IcePoint);
        Ice.SetActive(false);
        EffectDic.Add(Effect.Rain, Ice);
        EffectTrans.Add(Effect.Rain, IcePoint);
        Ice.transform.localPosition = Vector3.zero;
    }

    public GameObject GetEffect(Effect effect)
    {
        return EffectDic[effect];
    }

    public Transform GetEffectTransform(Effect effect)
    {
        return EffectTrans[effect];
    }

    public void ActiveIceRain()
    {
        GameObject ice_Rain = GetEffect(Effect.Rain);
        ice_Rain.transform.parent = EffectPool;
        ice_Rain.SetActive(false);
        ice_Rain.SetActive(true);

        StartCoroutine(IceRainEnable(ice_Rain));
    }

    public void ActiveTailCircle()
    {
        GameObject tail_circle = GetEffect(Effect.Tail_Circle);
        tail_circle.transform.GetChild(0).gameObject.SetActive(true);
        tail_circle.transform.GetChild(1).gameObject.SetActive(true);
        tail_circle.transform.rotation = Quaternion.Euler(0, 0, 0);
        tail_circle.transform.parent = EffectPool;
        tail_circle.SetActive(false);
        tail_circle.SetActive(true);

        driftSkill.SetAtk(wolf.GetAtk());

        StartCoroutine(TailCircleEnable(tail_circle));
    }

    public void ActiveSpike()
    {
        GameObject spike = GetEffect(Effect.Frost_Spike);
        spike.transform.parent = EffectPool;
        spike.SetActive(false);
        spike.SetActive(true);

        //StartCoroutine(SystemDie(spike));
    }

    public void ActiveMagic()
    {
        GameObject magic = GetEffect(Effect.Magic);
        magic.transform.parent = EffectPool;
        magic.SetActive(false);
        magic.SetActive(true);

        howlSkill.SetAtk(wolf.GetAtk());
        howlSkill.StartCoroutine(howlSkill.DelayDamage());
        StartCoroutine(MagicEnable(magic));
    }

    public void ActiveWave()
    {
        GameObject wave = GetEffect(Effect.Frost_Wave);
        wave.transform.parent = EffectPool;
        wave.SetActive(false);
        wave.SetActive(true);

        stampSkill.SetAtk(wolf.GetAtk());
        stampSkill.StartCoroutine(stampSkill.DelayDamage());
        StartCoroutine(WaveEnable(wave));
    }

    public void ActiveRing()
    {
        GameObject ring = GetEffect(Effect.Frost_Ring);
        ring.transform.parent = EffectPool;
        ring.SetActive(false);
        ring.SetActive(true);

        jumpSkill.SetAtk(wolf.GetAtk());
        jumpSkill.StartCoroutine(jumpSkill.DelayDamage());
        StartCoroutine(RingEnable(ring));
    }    

    public void ActiveCrystal()
    {
        GameObject crystal = GetEffect(Effect.Frost_Crystal);
        crystal.transform.parent = EffectPool;
        crystal.SetActive(false);
        crystal.SetActive(true);

        StartCoroutine(CrystalEnable(crystal));
    }    

    IEnumerator IceRainEnable(GameObject prefab)
    {
        yield return new WaitForSeconds(5.0f);
        prefab.SetActive(false);
        prefab.transform.parent = GetEffectTransform(Effect.Rain);
        prefab.transform.localPosition = Vector3.zero;
    }

    IEnumerator TailCircleEnable(GameObject prefab)
    {
        yield return new WaitForSeconds(2.0f);
        prefab.transform.GetChild(0).gameObject.SetActive(false);
        prefab.transform.GetChild(1).gameObject.SetActive(false);

        SphereCollider TailCirecle = prefab.GetComponent<SphereCollider>();
        TailCirecle.enabled = true;
        yield return new WaitForSeconds(1.0f);
        TailCirecle.enabled = false;

        prefab.SetActive(false);
        prefab.transform.parent = GetEffectTransform(Effect.Tail_Circle);
        prefab.transform.localPosition = Vector3.zero;
    }

    IEnumerator MagicEnable(GameObject prefab)
    {
        yield return new WaitForSeconds(4.0f);
        prefab.SetActive(false);
        prefab.transform.parent = GetEffectTransform(Effect.Magic);
        prefab.transform.localPosition = Vector3.zero;
    }

    IEnumerator WaveEnable(GameObject prefab)
    {
        ParticleSystem particleSystem = prefab.GetComponentInChildren<ParticleSystem>();
        while (particleSystem.isPlaying)
        {
            yield return new WaitForSeconds(0.5f);
        }
        prefab.transform.parent = GetEffectTransform(Effect.Frost_Wave);
        prefab.transform.localPosition = new Vector3(0, 0, 12f);
        Vector3 parentforward = GetEffectTransform(Effect.Frost_Wave).forward;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, parentforward);
        prefab.transform.rotation = rotation;

        yield break;
    }
    IEnumerator CrystalEnable(GameObject prefab)
    {
        yield return new WaitForSeconds(4.0f);
        prefab.SetActive(false);
        prefab.transform.parent = GetEffectTransform(Effect.Frost_Crystal);
        prefab.transform.localPosition = Vector3.zero;
    }
    IEnumerator RingEnable(GameObject prefab)
    {
        yield return new WaitForSeconds(3.5f);
        prefab.SetActive(false);
        prefab.transform.parent = GetEffectTransform(Effect.Frost_Ring);
        prefab.transform.localPosition = Vector3.zero;
    }


}

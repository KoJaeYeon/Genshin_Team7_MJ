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
        Tail_Circle
    }

    private Wolf wolf;
    private Jump jumpSkill;
    private Claw clawSkill;
    private Stamp stampSkill;
    private Howl howlSkill;
    private Drift driftSkill;

    public GameObject Frost_Wave;
    public GameObject Frost_Ring;
    public GameObject Frost_Spike;
    public GameObject Frost_Crystal;
    public GameObject Magic_Circle;
    public GameObject Tail_Circle;

    public Transform WavePoint;
    public Transform RingPoint;
    public Transform SpikePoint;
    public Transform CrystalPoint;
    public Transform MagicPoint;
    public Transform TailPoint;

    private Dictionary<Effect, GameObject> EffectDic;
    private Dictionary<Effect, Transform> EffectTrans;
    
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        wolf = GetComponent<Wolf>();
        EffectDic = new Dictionary<Effect, GameObject>();
        EffectTrans = new Dictionary<Effect, Transform>();

        GameObject frost_Wave = Instantiate(Frost_Wave, WavePoint);
        stampSkill = frost_Wave.GetComponent<Stamp>();
        frost_Wave.SetActive(false);
        EffectDic.Add(Effect.Frost_Wave, frost_Wave);
        EffectTrans.Add(Effect.Frost_Wave, WavePoint);
        frost_Wave.transform.localPosition = new Vector3(0, 0, 12f);
        Vector3 parentforward = GetEffectTransform(Effect.Frost_Wave).forward;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.right, parentforward);
        frost_Wave.transform.rotation = rotation;

        GameObject frost_Ring = Instantiate(Frost_Ring, RingPoint);
        jumpSkill = frost_Ring.GetComponent<Jump>();
        frost_Ring.SetActive(false);
        EffectDic.Add(Effect.Frost_Ring , frost_Ring);
        EffectTrans.Add(Effect.Frost_Ring, RingPoint);
        frost_Ring.transform.localPosition = Vector3.zero;

        GameObject frost_Spike = Instantiate(Frost_Spike, SpikePoint);
        frost_Spike.SetActive(false);
        EffectDic.Add(Effect.Frost_Spike , frost_Spike);
        EffectTrans.Add(Effect.Frost_Spike, SpikePoint);
        frost_Spike.transform.localPosition = Vector3.zero;

        GameObject frost_Crystal = Instantiate(Frost_Crystal, CrystalPoint);
        frost_Crystal.SetActive(false);
        EffectDic.Add(Effect.Frost_Crystal , frost_Crystal);
        EffectTrans.Add(Effect.Frost_Crystal, CrystalPoint);
        frost_Crystal.transform.localPosition = Vector3.zero;

        GameObject Magic = Instantiate(Magic_Circle, MagicPoint);
        howlSkill = Magic.GetComponent<Howl>();
        Magic.SetActive(false);
        EffectDic.Add(Effect.Magic, Magic);
        EffectTrans.Add(Effect.Magic, MagicPoint);
        Magic.transform.localPosition = Vector3.zero;

        GameObject tail_circle = Instantiate(Tail_Circle, TailPoint);
        tail_circle.SetActive(false);
        EffectDic.Add(Effect.Tail_Circle, tail_circle);
        EffectTrans.Add(Effect.Tail_Circle, TailPoint);
        tail_circle.transform.localPosition = Vector3.zero;
    }

    public GameObject GetEffect(Effect effect)
    {
        return EffectDic[effect];
    }

    public Transform GetEffectTransform(Effect effect)
    {
        return EffectTrans[effect];
    }

    public void ActiveTailCircle()
    {
        GameObject tail_circle = GetEffect(Effect.Tail_Circle);
        tail_circle.transform.GetChild(2).gameObject.SetActive(true);
        tail_circle.transform.GetChild(6).gameObject.SetActive(true);
        tail_circle.transform.parent = null;
        tail_circle.SetActive(false);
        tail_circle.SetActive(true);

        driftSkill.SetAtk(wolf.GetAtk());

        StartCoroutine(TailCircleEnable(tail_circle));
    }

    public void ActiveSpike()
    {
        GameObject spike = GetEffect(Effect.Frost_Spike);
        spike.transform.parent = null;
        spike.SetActive(false);
        spike.SetActive(true);

        //StartCoroutine(SystemDie(spike));
    }

    public void ActiveMagic()
    {
        GameObject magic = GetEffect(Effect.Magic);
        magic.transform.parent = null;
        magic.SetActive(false);
        magic.SetActive(true);

        howlSkill.SetAtk(wolf.GetAtk());
        howlSkill.StartCoroutine(howlSkill.DelayDamage());
        StartCoroutine(MagicEnable(magic));
    }

    public void ActiveWave()
    {
        GameObject wave = GetEffect(Effect.Frost_Wave);
        wave.transform.parent = null;
        wave.SetActive(false);
        wave.SetActive(true);

        stampSkill.SetAtk(wolf.GetAtk());
        stampSkill.StartCoroutine(stampSkill.DelayDamage());
        StartCoroutine(WaveEnable(wave));
    }

    public void ActiveRing()
    {
        GameObject ring = GetEffect(Effect.Frost_Ring);
        ring.transform.parent = null;
        ring.SetActive(false);
        ring.SetActive(true);

        jumpSkill.SetAtk(wolf.GetAtk());
        jumpSkill.StartCoroutine(jumpSkill.DelayDamage());
        StartCoroutine(RingEnable(ring));
    }    

    public void ActiveCrystal()
    {
        GameObject crystal = GetEffect(Effect.Frost_Crystal);
        crystal.transform.parent = null;
        crystal.SetActive(false);
        crystal.SetActive(true);

        StartCoroutine(CrystalEnable(crystal));
    }    

    IEnumerator TailCircleEnable(GameObject prefab)
    {
        yield return new WaitForSeconds(2.0f);
        
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
        ParticleSystem particleSystem = prefab.GetComponentInChildren<ParticleSystem>();
        while (particleSystem.isPlaying)
        {
            yield return new WaitForSeconds(0.5f);
        }
        prefab.transform.parent = GetEffectTransform(Effect.Frost_Ring);
        prefab.transform.localPosition = Vector3.zero;
        yield break;
    }


}

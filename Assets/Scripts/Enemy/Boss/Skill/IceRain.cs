using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceRain : BossSkill
{
    private float Ice_Atk;
    private float iceDamage;
    private AndriusHowlData _data;

    public IceRain()
    {
        _data = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusHowlData>("AndriusHowlData");
        iceDamage = _data.GetData(DataList.SKillDamage_ice);
    }

    public override void SetAtk(float atk)
    {
        Ice_Atk = iceDamage * atk;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Character player = other.gameObject.GetComponentInChildren<Character>();
            player.TakeDamage(Ice_Atk);
        }
    }
}

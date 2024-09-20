using System.Collections;
using UnityEngine;

public class AndriusParalyzation : IPattern
{
    private Andrius _andrius;
    private Animator _animator;
    private WaitForSeconds _changeTime; 
    private AndriusParalyzationData _paralyzationData;
    private bool _isChangeState;

    private float __paralyzationValue;

    public AndriusParalyzation()
    {
        _paralyzationData = EnemyCSVLoder.Instance.GetAndriusCSVData<AndriusParalyzationData>("AndriusParalyzationData");
        _changeTime = new WaitForSeconds(_paralyzationData.ChangeTime);
        __paralyzationValue = _paralyzationData.ParalyzationValue;
        Debug.Log($"AndriusParalyzation{_paralyzationData.ParalyzationValue},{_paralyzationData.ChangeTime}");
    }

    public void InitializePattern(Andrius andrius)
    {
        if(_andrius == null)
        {
            _andrius = andrius;
            _animator = _andrius.GetComponent<Animator>();
        }

        _animator.SetBool("Idle", true);
        _isChangeState = false;
        _andrius.StartCoroutine(ChangeTimer());
    }

    public void UpdatePattern()
    {
        if (_isChangeState)
        {
            _andrius.State.ChangeState(BossState.Attack);
        }
    }

    public void ExitPattern()
    {
        _animator.SetBool("Idle", false);
        _andrius.Paralyzation = __paralyzationValue;
    }

    private IEnumerator ChangeTimer()
    {
        yield return _changeTime;
        _isChangeState = true;
    }
}

using System.Collections;
using UnityEngine;

public class AndriusParalyzation : IPattern
{
    private Andrius _andrius;
    private Animator _animator;
    private WaitForSeconds _changeTime = new WaitForSeconds(4f);
    private bool _isChangeState;

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
        _andrius.Paralyzation = 100f;
    }

    private IEnumerator ChangeTimer()
    {
        yield return _changeTime;
        _isChangeState = true;
    }
}

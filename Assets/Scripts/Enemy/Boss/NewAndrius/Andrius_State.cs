using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public abstract class Andrius_State : IAndriusBase
{
    protected Andrius _andrius;
    protected Animator _animator;
    protected NavMeshAgent _agent;
    protected Rigidbody _rigidbody;
    protected Rig _rig;
    protected BossStateMachine _state;
    protected Transform _playerTransform;

    protected AndriusParalyzationData _paralyzationData;
    protected AndriusAttackData _attackData;
    protected AndriusWalkData _walkData;
    protected AndriusClawData _clawData;
    protected AndriusChargeData _chargeData;
    protected AndriusJumpData _jumpData;
    protected AndriusStampData _stampData;

    protected readonly int _idle = Animator.StringToHash("Idle");
    protected readonly int _hit = Animator.StringToHash("Hit");
    protected readonly int _turnRight = Animator.StringToHash("TurnRight");
    protected readonly int _turnLeft = Animator.StringToHash("TurnLeft");
    protected readonly int _back = Animator.StringToHash("JumpBack");
    protected readonly int _clawRigt = Animator.StringToHash("ClawR");
    protected readonly int _clawLeft = Animator.StringToHash("ClawL");
    protected readonly int _claw_RightDrift = Animator.StringToHash("ClawR_Drift");
    protected readonly int _claw_LeftDrift = Animator.StringToHash("ClawL_Drift");
    protected readonly int _drift_Right = Animator.StringToHash("DriftR");
    protected readonly int _drift_Left = Animator.StringToHash("DriftL");
    protected readonly int _run = Animator.StringToHash("isRun");
    protected readonly int _scramRight = Animator.StringToHash("ScramRight");
    protected readonly int _scramLeft = Animator.StringToHash("ScramLeft");
    protected readonly int _jump = Animator.StringToHash("JumpAttack");
    protected readonly int _stamp = Animator.StringToHash("Stamp");
    protected readonly int _howl = Animator.StringToHash("Howl");
    protected readonly int _iceRain = Animator.StringToHash("IceRain");

    public Andrius_State(Andrius andrius)
    {
        _andrius = andrius;
        _animator = _andrius.GetComponent<Animator>();
        _agent = _andrius.GetComponent<NavMeshAgent>();
        _rigidbody = _andrius.GetComponent<Rigidbody>();
        _state = _andrius.GetComponent<BossStateMachine>();
        _rig = _andrius.transform.GetComponentInChildren<Rig>();

        GetData();

        _playerTransform = _andrius.PlayerTransform;
    }

    public void GetData()
    {
        _paralyzationData = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusParalyzationData>("AndriusParalyzationData");
        _attackData = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusAttackData>("AndriusAttackData");
        _walkData = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusWalkData>("AndriusWalkData");
        _clawData = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusClawData>("AndriusClawData");
        _chargeData = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusChargeData>("AndriusChargeData");
        _jumpData = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusJumpData>("AndriusJumpData");
        _stampData = EnemyCSVLoader.Instance.GetAndriusCSVData<AndriusStampData>("AndriusStampData");
    }
    public virtual void StateEnter()
    {
        if(_andrius.Paralyzation <= 0)
        {
            _andrius.IsAction = false;

            _state.ChangeState(BossState.Idle);

            return;
        }
    }
    public virtual void StateFixedUpdate() { }
    public virtual void StateExit() { }
    public virtual void OnTriggerEnter(Collider other) { }
}

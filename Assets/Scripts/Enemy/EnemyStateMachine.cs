using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Idle,
    Move,
    Attack
}

public class EnemyStateMachine : MonoBehaviour
{
    private Dictionary<EnemyState, IBaseState> EnemyStateDic;
    IBaseState State;

    void Start()
    {
        InitState();
    }

    void Update()
    {
        State.StateUpdate();
    }

    public void InitState()
    {
        EnemyStateDic = new Dictionary<EnemyState, IBaseState>();
        State = EnemyStateDic[EnemyState.Idle];
        State.StateEnter();
    }

    public void AddState(EnemyState addState, IBaseState State)
    {
        EnemyStateDic.Add(addState, State);
    }

    public void ChangeState(EnemyState changeState)
    {
        State.StateExit();
        State = EnemyStateDic[changeState];
        State.StateEnter();
    }

    private void OnCollisionEnter(Collision collision)
    {
        State.OnCollisionEnter(collision);
    }

    //...필요하면 추가 예정
}

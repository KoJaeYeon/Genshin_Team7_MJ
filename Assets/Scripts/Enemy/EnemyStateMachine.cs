using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    Idle,
    Move,
    Trace
}

public class EnemyStateMachine : MonoBehaviour
{
    private Dictionary<EnemyState, BaseState> EnemyStateDic = new Dictionary<EnemyState, BaseState>();  
    BaseState State;

    private void Start()
    {
        InitState();
    }

    void Update()
    {
        State.StateUpDate();
    }

    public void InitState()
    {
        State = EnemyStateDic[EnemyState.Idle];
        State.StateEnter();
    }

    public void AddState(EnemyState addState, BaseState State)
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

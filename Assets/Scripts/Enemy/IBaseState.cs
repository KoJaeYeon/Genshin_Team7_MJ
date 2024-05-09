using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseState
{
    public void StateEnter();
    public void StateUpdate();
    public void StateExit();
    public void OnCollisionEnter(Collision collision);
}

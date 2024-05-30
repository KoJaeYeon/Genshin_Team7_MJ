using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseState
{
    public abstract void StateEnter();
    public abstract void StateUpDate();
    public abstract void StateExit();
    public virtual void OnCollisionEnter(Collision collision) { }
}

public abstract class BossBaseState
{
    public abstract void StateEnter();
    public abstract void StateFixedUpdate();
    public abstract void StateExit();
    public virtual void OnTriggerEnter(Collider other) { }
}
using UnityEngine;

public abstract class AndriusState : BossBaseState
{
    protected Andrius _andrius;
    public AndriusState(Andrius andrius)
    {
        _andrius = andrius;
    }
}

public class AndriusIdleState : AndriusState
{
    public AndriusIdleState(Andrius andrius) : base(andrius) { }

    public override void StateEnter()
    {
        _andrius.SetPattern(AndriusPattern.Idle);
        _andrius.CurrentPattern.InitializePattern(_andrius);
    }
    public override void StateFixedUpdate()
    {
        _andrius.CurrentPattern.UpdatePattern();
    }

    public override void StateExit()
    {
        _andrius.CurrentPattern.ExitPattern();
    }
   
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _andrius.BossAnimator.SetTrigger("Hit");
        }
    }
}

public class AndriusWalkState : AndriusState
{
    public AndriusWalkState(Andrius andrius) : base (andrius) { }

    public override void StateEnter()
    {
        _andrius.SetPattern(AndriusPattern.Move);
        _andrius.CurrentPattern.InitializePattern(_andrius);
    }
    public override void StateFixedUpdate()
    {
        _andrius.CurrentPattern.UpdatePattern();
    }

    public override void StateExit()
    {
        _andrius.CurrentPattern.ExitPattern();
    }
}

public class AndriusAttackState : AndriusState
{
    public AndriusAttackState(Andrius andrius) : base(andrius) { }

    public override void StateEnter()
    {
        _andrius.SetPattern(AndriusPattern.Attack);
        _andrius.CurrentPattern.InitializePattern(_andrius);
    }

    public override void StateFixedUpdate()
    {
        _andrius.CurrentPattern.UpdatePattern();
    }

    public override void StateExit()
    {
        _andrius.CurrentPattern.ExitPattern();
    }
}

public class Andrius_Howl : AndriusState
{
    public Andrius_Howl(Andrius andrius) : base(andrius) { }
    public override void StateEnter()
    {
        _andrius.SetPattern(AndriusPattern.Howl);
        _andrius.CurrentPattern.InitializePattern(_andrius);
    }
}

public class Andrius_Stamp : AndriusState
{
    public Andrius_Stamp(Andrius andrius) : base(andrius) { }
    public override void StateEnter()
    {
        _andrius.SetPattern(AndriusPattern.Stamp);
        _andrius.CurrentPattern.InitializePattern(_andrius);
    }
    public override void StateFixedUpdate()
    {
        _andrius.CurrentPattern.UpdatePattern();
    }
}

public class Andrius_Jump : AndriusState
{
    public Andrius_Jump(Andrius andrius) : base(andrius) { }

    public override void StateEnter()
    {
        _andrius.SetPattern(AndriusPattern.Jump);
        _andrius.CurrentPattern.InitializePattern(_andrius);
    }
    public override void StateFixedUpdate()
    {
        _andrius.CurrentPattern.UpdatePattern();
    }

    public override void StateExit()
    {
        _andrius.CurrentPattern.ExitPattern();
    }
}


public class Andrius_Claw : AndriusState
{
    public Andrius_Claw(Andrius andrius) : base(andrius) { }
    private bool _isHit = false;
    private float claw_Atk = 1f;

    public override void StateEnter()
    {
        _andrius.SetPattern(AndriusPattern.Claw);
        _andrius.CurrentPattern.InitializePattern(_andrius);
    }
    public override void StateExit()
    {
        _isHit = false;
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !_isHit)
        {
            _isHit = true;

            Character player = other.gameObject.GetComponentInChildren<Character>();

            if (player != null)
            {
                player.TakeDamage(_andrius.GetAtk() * claw_Atk);
            }
        }
    }
}

public class Andrius_Drift : AndriusState
{
    public Andrius_Drift(Andrius andrius) : base(andrius) { }
    public override void StateEnter()
    {
        _andrius.SetPattern(AndriusPattern.Drift);
        _andrius.CurrentPattern.InitializePattern(_andrius);
    }
}

public class Andrius_Charge : AndriusState
{
    public Andrius_Charge(Andrius andrius) : base(andrius) { }

    private bool _isHit;
    private float charge_Atk = 2f;

    public override void StateEnter()
    {
        _andrius.SetPattern(AndriusPattern.Charge);
        _andrius.CurrentPattern.InitializePattern(_andrius);
    }

    public override void StateExit()
    {
        _isHit = false;
        _andrius.CurrentPattern.ExitPattern();
    }

    public override void StateFixedUpdate()
    {
        _andrius.CurrentPattern.UpdatePattern();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !_isHit)
        {
            _isHit = true;

            Character player = other.gameObject.GetComponentInChildren<Character>();

            if (player != null)
            {
                player.TakeDamage(_andrius.GetAtk() * charge_Atk);
            }
        }
    }
}
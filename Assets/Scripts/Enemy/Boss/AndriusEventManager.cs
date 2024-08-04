using System;

public class AndriusEventManager
{
    private static AndriusEventManager _instance;
    private IAndriusClawEvent _andriusClawEvent;
    private IAndriusChargeEvent _andriusChargeEvent;

    public static AndriusEventManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new AndriusEventManager();
            }

            return _instance;
        }
    }

    public void RegisterClawEvent(IAndriusClawEvent eventHandler)
    {
        _andriusClawEvent = eventHandler;
    }
    public void RegisterChargeEvent(IAndriusChargeEvent eventHandler)
    {
        _andriusChargeEvent = eventHandler;
    }

    public void AddEvent_LeftClawEvent(Action callBack)
    {
        _andriusClawEvent.LeftClawEvent(callBack);
    }

    public void AddEvent_RightClawEvent(Action callBack)
    {
        _andriusClawEvent.RightClawEvent(callBack);
    }

    public void AddEvent_OnChargeColliderEvent(Action callBack)
    {
        _andriusChargeEvent.OnChargeColliderEvent(callBack);
    }
    public void AddEvent_OffColliderEvent(Action callBack)
    {
        _andriusChargeEvent.OffChargeColliderEvent(callBack);
    }
}

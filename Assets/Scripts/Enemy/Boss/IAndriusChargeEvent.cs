using System;   

public interface IAndriusChargeEvent
{
    public void OnChargeColliderEvent(Action callBack);
    public void OffChargeColliderEvent(Action callBack);
}

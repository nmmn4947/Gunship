using System;
using UnityEngine;

public class GunShipMoveAction : Napadol.Tools.ActionPattern.Action
{

    public GunShipMoveAction(GameObject subject, bool blocking, float delay, float duration, Func<float, float> easingFunction) : base(subject, blocking, delay, duration, easingFunction)
    {
        
    }

    protected override bool UpdateLogicUntilDone(float dt)
    {
        throw new NotImplementedException();
    }
}

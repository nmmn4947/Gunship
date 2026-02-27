using System;
using UnityEngine;

public class GunShip2DMoveAction : Napadol.Tools.ActionPattern.Action
{
    Rigidbody2D rigidbody2D;
    
    public GunShip2DMoveAction(GameObject subject, Vector2 moveInput) : base()
    {
        rigidbody2D = subject.GetComponent<Rigidbody2D>();
        if (subject == null)
        {
            Debug.LogError("Can't Find rb2D");
        }
    }

    protected override bool UpdateLogicUntilDone(float dt)
    {
        throw new NotImplementedException();
    }
}

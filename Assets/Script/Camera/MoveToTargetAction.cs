using UnityEngine;

public class MoveToTargetAction : Napadol.Tools.ActionPattern.Action
{
    private Transform target;
    private Vector3 originalPosition;
    private bool dontMoveZ = false;
    
    public MoveToTargetAction(GameObject subject, float duration, Transform target) : base(subject, duration)
    {
        this.target = target;
    }

    #region Builders

    public MoveToTargetAction DontMoveZ()
    {
        dontMoveZ = true;
        return this;
    }

    #endregion
    
    protected override void RunOnceBeforeUpdate()
    {
        originalPosition = subject.transform.position;
    }
    
    protected override bool UpdateLogicUntilDone(float dt)
    {
        Vector3 newPos = new Vector3();

        newPos = dontMoveZ ? new Vector3(target.position.x, target.position.y, originalPosition.z) : new Vector3(target.position.x, target.position.y, target.position.z);
        
        subject.transform.position = Vector3.LerpUnclamped(originalPosition, newPos, easingTimePasses);
        return percentageDone >= 1f;
    }
}

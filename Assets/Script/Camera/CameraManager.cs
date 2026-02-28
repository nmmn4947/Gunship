using System;
using System.Collections.Generic;
using Napadol.Tools;
using UnityEngine;

public class CameraManager : ActionListManager
{
    [SerializeField] private List<Transform> targets;
    private Transform target;
    
    private Camera _camera;
    
    private float borderLeft;
    private float borderRight;
    private float borderTop;
    private float borderDown;

    private float DEFAULTDURATION = 0.75f;

    void Start()
    {
        _camera = GetComponent<Camera>();

        borderLeft  = _camera.ViewportToWorldPoint(new Vector3(0.25f, 0, 0)).x;
        borderRight = _camera.ViewportToWorldPoint(new Vector3(0.75f, 0, 0)).x;
        borderDown  = _camera.ViewportToWorldPoint(new Vector3(0, 0.25f, 0)).y; 
        borderTop   = _camera.ViewportToWorldPoint(new Vector3(0, 0.75f, 0)).y;
        
        target = new GameObject("Target").transform;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void LateUpdate()
    {
        target.position = CalculateTargetPosition();
        if (target.position.x < borderLeft ||
            target.position.x > borderRight ||
            target.position.y < borderTop ||
            target.position.y > borderDown)
        {
            actionList.AddAction(new MoveToTargetAction(_camera.gameObject, DEFAULTDURATION, target).DontMoveZ().Easer(Easing.EaseOutSine));
        }
    }

    private Vector3 CalculateTargetPosition()
    {
        if (targets.Count == 1)
            return targets[0].position;

        Vector3 middle = Vector3.zero;
        foreach (Transform t in targets)
        {
            middle += t.position;
        }
        return middle / targets.Count;
    }
}

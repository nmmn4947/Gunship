using System;
using System.Collections.Generic;
using Napadol.Tools;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class CameraManager : ActionListManager
{
    [SerializeField] private List<Transform> targets;
    [SerializeField] private PlayerManager playerManager;
    private Transform target;
    
    private Camera _camera;
    
    private float borderLeft;
    private float borderRight;
    private float borderTop;
    private float borderDown;
    
    private float DEFAULTDURATION = 1f;
    
    private float currentVelocity;

    void Start()
    {
        _camera = GetComponent<Camera>();
        UpdateBorders();
        target = new GameObject("Target").transform;
    }

    protected override void Update()
    {
        base.Update();
        ExpandDependOnVelocity(target.position);
    }

    private void LateUpdate()
    {
        target.position = CalculateTargetPosition();
        //actionList.AddAction(new MoveToTargetAction(_camera.gameObject, DEFAULTDURATION, target).DontMoveZ().Easer(Easing.EaseOutSine));

        MoveTowardsTargetWithBoundary(target.position);
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

    private void UpdateBorders()
    {
        borderLeft  = _camera.ViewportToWorldPoint(new Vector3(0.25f, 0, 0)).x;
        borderRight = _camera.ViewportToWorldPoint(new Vector3(0.75f, 0, 0)).x;
        borderDown  = _camera.ViewportToWorldPoint(new Vector3(0, 0.45f, 0)).y; 
        borderTop   = _camera.ViewportToWorldPoint(new Vector3(0, 0.55f, 0)).y;
    }

    private void MoveWhenOutOfBorder(Vector3 targetPosition)
    {
        if (targetPosition.x < borderLeft ||
            targetPosition.x > borderRight)
        {
            actionList.AddAction(new MoveToTargetAction(_camera.gameObject, DEFAULTDURATION, target).DontMoveZ().Easer(Easing.EaseOutSine));
        }

        if (targetPosition.y < borderDown ||
            targetPosition.y > borderTop)
        {
            actionList.AddAction(new MoveToTargetAction(_camera.gameObject, DEFAULTDURATION/2f, target).DontMoveZ().Easer(Easing.EaseOutSine));
        }
    }
    
    private void ExpandWhenOutOfBorder(Vector3 targetPosition)
    {
        if (targetPosition.x < borderLeft ||
            targetPosition.x > borderRight ||
            targetPosition.y < borderDown ||
            targetPosition.y > borderTop)
        {
            float newSize = Vector2.Distance(_camera.gameObject.transform.position, targetPosition);
            if (newSize < 10f)
            {
                newSize = 10f;
            }
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize,  newSize, (newSize) * Time.deltaTime);
        }
        else
        {
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, 10, 3f * Time.deltaTime);
        }
    }

    private void ExpandDependOnVelocity(Vector3 targetPosition)
    {
        currentVelocity = playerManager.gameObject.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
        /*if (currentVelocity > playerManager._currentShipData.maxSpeed)
        {
            float newSize = Vector2.Distance(_camera.gameObject.transform.position, targetPosition);
            if (newSize < 12f)
            {
                newSize = 12f;
            }

            if (currentVelocity > 0 && currentVelocity < 5)
            {
                newSize = 12 + currentVelocity;
            }
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize,  newSize, (currentVelocity) * Time.deltaTime);
        }
        else
        {
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, 12, currentVelocity * Time.deltaTime);
        }*/
        float newValue = Mathf.Lerp(0, 10, currentVelocity / 100f);
        _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize,  10 + newValue, (currentVelocity) * Time.deltaTime);
    }

    private void MoveTowardsTargetWithBoundary(Vector3 targetPosition)
    {
        float left = _camera.ViewportToWorldPoint(new Vector3(0f, 0, 0)).x;
        float right = _camera.ViewportToWorldPoint(new Vector3(1f, 0, 0)).x;
        float bot = _camera.ViewportToWorldPoint(new Vector3(0, 0f, 0)).y; 
        float top = _camera.ViewportToWorldPoint(new Vector3(0, 1f, 0)).y;
        
        float step = ((currentVelocity*0.75f) + Vector3.Distance(targetPosition, _camera.transform.position)) * Time.deltaTime;
        /*if (Vector3.Distance(targetPosition, _camera.transform.position) > 10f)
        {
            step = (currentVelocity + Vector3.Distance(targetPosition, _camera.transform.position)) * Time.deltaTime;
        }
        else
        {
            step = currentVelocity * Time.deltaTime;
        }*/
        
        float halfWidth  = (right - left) / 2f;
        float halfHeight = (top - bot) / 2f;
        
        float x = Mathf.MoveTowards(_camera.transform.position.x, targetPosition.x, step);
        x = Mathf.Clamp(x, -100f + halfWidth, 100f - halfWidth);
        
        float y = Mathf.MoveTowards(_camera.transform.position.y, targetPosition.y, step);
        y = Mathf.Clamp(y, -100f + halfHeight, 100f - halfHeight);

        
        Vector3 newPos = new Vector3(x,y, _camera.transform.position.z);
        _camera.transform.position = newPos;
    }
}

using System;
using UnityEngine;

public class MissileMovement : MonoBehaviour
{
    [SerializeField] private float _jerk;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAngularSpeed;
    
    private Transform target;
    private Rigidbody2D rb2D;
    
    private float currentAcceleration = 0;
    private float currentSpeed = 0;
    
    private PLACEHOLDER_TARGET placeholderTarget;

    private void Start()
    {
        placeholderTarget = PLACEHOLDER_TARGET.instance;
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }
    
    private void Accelerates()
    {
        if (Mathf.Abs(currentAcceleration) < maxAcceleration)
        {
            currentAcceleration += _jerk * Time.fixedDeltaTime;
        }
    }

    private void Rotates()
    {
        Vector2 dir = placeholderTarget.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        float angleDiff = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);

        // Rotate directly instead of AddTorque
        float rotation = Mathf.Clamp(angleDiff, -maxAngularSpeed, maxAngularSpeed) * Time.fixedDeltaTime;
        transform.Rotate(0, 0, rotation);
    }
    
    private void FixedUpdate()
    {
        Accelerates();
        Rotates();
        if (currentSpeed <= maxSpeed)
        {
            currentSpeed += currentAcceleration * Time.fixedDeltaTime;
        }
        rb2D.linearVelocity = this.transform.up * currentSpeed;
        Debug.Log(rb2D.linearVelocity.magnitude);
    }
}

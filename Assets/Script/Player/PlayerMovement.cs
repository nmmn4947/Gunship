using System;
using UnityEngine;

public class PlayerMovement
{
    //The philosophy here is to have functions about moving only
    //and let the manager choose what to do
    private ShipData currentShip;
    private Rigidbody2D playerRB2D;
    private Transform playerTransform;
    
    private Vector2 direction;
    private float currentAcceleration = 0.0f;
    
    private float currentSpeed;
    private float accelerationTimer = 0.0f;
    private float pulsePower;
    private float pulseChargeTimer = 0.0f;
    
    private float maxJerk = 0.0f;
    private float jerk = 0.0f;
    
    private bool isAccelerating = false;
    private bool isCharging = false;
    private Vector2 knockbackVelocity = Vector2.zero;
    
    public void SetUp(GameObject ship, ShipData data, Rigidbody2D playerRB)
    {
        playerRB2D = playerRB;
        playerTransform = ship.GetComponent<Transform>();
        currentShip = data;
        maxJerk = data.maxSpeed / data.timeUntilMaxAcceleration;
    }

    public void UpdateMovement()
    {
        Drag();
        currentSpeed += currentAcceleration * Time.fixedDeltaTime;
        
        knockbackVelocity = Vector2.MoveTowards(knockbackVelocity, Vector2.zero, 25f * Time.fixedDeltaTime);

        if (!isCharging)
        {
            Vector2 finalVelocity = ((Vector2)playerTransform.up * currentSpeed) + knockbackVelocity;
            finalVelocity = Vector2.ClampMagnitude(finalVelocity, currentShip.maxSpeed);
            playerRB2D.linearVelocity = finalVelocity;
            
            //playerRB2D.AddForce(playerTransform.up * 5f,  ForceMode2D.Force);
        }
    }
    
    public void ApplyKnockback(Vector2 force)
    {
        knockbackVelocity += force;
    }

    //Jerk -> just to lerp the acceleration
    public void Accelerates(float moveInput, bool isLow)
    {
        if (currentSpeed > currentShip.maxSpeed)
        {
            isAccelerating = false;
            return;
        }
        
        if (moveInput == 0.0f)
        {
            accelerationTimer = 0.0f;
            isAccelerating = false;
            return;
        }
        else
        {
            if (moveInput > 0.0f)
            {
                moveInput = 1.0f;
                if (isLow)
                {
                    moveInput *= 0.5f;
                }
            }
            else
            {
                moveInput = -1.0f;
                if (isLow)
                {
                    moveInput *= 0.5f;
                }
            }
        }
        
        isAccelerating = true;
        
        accelerationTimer += Time.fixedDeltaTime;
        if (Mathf.Abs(currentAcceleration) >= currentShip.maxAcceleration)
        {
            jerk = 0.0f;
        }
        else
        {
            jerk = moveInput * maxJerk;
        }

        if (currentSpeed < currentShip.maxSpeed)
        {
            currentAcceleration += jerk * Time.fixedDeltaTime;
        }
    }

    private void Drag()
    {
        if (isAccelerating)
        {
            if (currentSpeed >= currentShip.maxSpeed)
            {
                currentAcceleration = 0.0f;
            }
            return;
        }

        currentAcceleration = Mathf.MoveTowards(currentAcceleration, 0.0f, (currentShip.dragForce) * Time.fixedDeltaTime);

        currentSpeed = Mathf.MoveTowards(currentSpeed, 0.0f, (currentShip.dragForce) * Time.fixedDeltaTime);
        if (currentSpeed > currentShip.maxSpeed)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0.0f, (currentShip.dragForce) * Time.fixedDeltaTime);
        }
    }

    public void AngularAccelerates(float moveInput, bool isLow)
    {
        if (moveInput >= -0.001f && moveInput <= 0.001f)
        {
            playerRB2D.angularVelocity = 0.0f;
            return;
        }
        
        //if moveInput is < 0.9 meaning A or D and W or S is press at the same time 
        if (moveInput < 0f)
        {
            moveInput = -1.0f;
            if (isLow)
            {
                moveInput *= 0.5f;
            }
        }
        else
        {
            moveInput = 1.0f;
            if (isLow)
            {
                moveInput *= 0.5f;
            }
        }
        
        playerRB2D.AddTorque(-moveInput * currentShip.torque);
    }

    public void DriftPulseCharge()
    {
        pulseChargeTimer += Time.deltaTime;
        isCharging = true;
        pulsePower = Mathf.Lerp(0.0f, currentShip.pulseMaxPower, pulseChargeTimer/currentShip.pulseChargeDuration);
    }

    public void DriftPulse()
    {
        //playerRB2D.AddForce(playerTransform.up * pulsePower, ForceMode2D.Impulse);
        direction = playerTransform.up;
        pulseChargeTimer = 0f;
        isCharging = false;
        currentSpeed += pulsePower;
    }

    public void ResetMovement()
    {
        if (currentShip == null)
        {
            return;
        }
        playerRB2D.linearVelocity = Vector2.zero;
        currentSpeed = 0.0f;
        currentAcceleration = 0.0f;
        jerk = 0.0f;
    }
}

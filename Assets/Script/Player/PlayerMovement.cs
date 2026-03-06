using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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

    private float oldInput = 0f;
    public void SetUp(GameObject ship, ShipData data)
    {
        playerRB2D = ship.GetComponent<Rigidbody2D>();
        playerTransform = ship.GetComponent<Transform>();
        currentShip = data;
        maxJerk = data.maxSpeed / data.timeUntilMaxAcceleration;
        direction = playerTransform.up;
    }

    private void FixedUpdate()
    {
        Drag();
        currentSpeed += currentAcceleration * Time.fixedDeltaTime;
        playerRB2D.linearVelocity = direction * currentSpeed;
        Debug.Log(currentSpeed + ":" + currentAcceleration);
    }

    //Jerk -> just to lerp the acceleration
    public void Accelerates(float moveInput)
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
        
        isAccelerating = true;

        float checkInput = 0f;
        if (moveInput > 0.5f)
        {
            direction = playerTransform.up;
            checkInput = 1f;
        }
        else if (moveInput < -0.5f)
        {
            direction = -playerTransform.up;
            checkInput = -1f;
        }
        
        /*if (!Mathf.Approximately(oldInput, checkInput))
        {
            currentSpeed = 0;
        }*/
        
        accelerationTimer += Time.fixedDeltaTime;
        if (Mathf.Abs(currentAcceleration) >= currentShip.maxAcceleration)
        {
            jerk = 0.0f;
        }
        else
        {
            jerk = maxJerk;
        }

        if (currentSpeed < currentShip.maxSpeed)
        {
            currentAcceleration += jerk * Time.fixedDeltaTime;
        }

        
        if (moveInput > 0.5f)
        {
            direction = playerTransform.up;
            oldInput = 1f;
        }
        else if (moveInput < -0.5f)
        {
            direction = -playerTransform.up;
            oldInput = -1f;
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

    public void AngularAccelerates(float moveInput)
    {
        if (moveInput >= -0.001f && moveInput <= 0.001f)
        {
            playerRB2D.angularVelocity = 0.0f;
            return;
        }
        
        //if moveInput is < 0.9 meaning A or D and W or S is press at the same time 
        if (moveInput > -0.9f && moveInput < 0f)
        {
            moveInput = -1.0f;
        }
        else if (moveInput < 0.9f && moveInput > 0f)
        {
            moveInput = 1.0f;
        }
        
        playerRB2D.AddTorque(-moveInput * currentShip.maxAngularAcceleration);
    }

    public void DriftPulseCharge()
    {
        pulseChargeTimer += Time.deltaTime;
        pulsePower = Mathf.Lerp(0.0f, currentShip.pulseMaxPower, pulseChargeTimer/currentShip.pulseChargeDuration);
    }

    public void DriftPulse()
    {
        //playerRB2D.AddForce(playerTransform.up * pulsePower, ForceMode2D.Impulse);
        direction = playerTransform.up;
        pulseChargeTimer = 0f;
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

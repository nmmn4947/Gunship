using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //The philosophy here is to have functions about moving only
    //and let the manager choose what to do
    public ShipData currentShip { private get; set; }
    private Rigidbody2D playerRB2D;
    private Transform playerTransform;
    private float currentAcceleration;
    private float currentSpeed;
    private float trackAcceleration = 0.0f;
    private float accelerationTimer = 0.0f;

    public void SetUp(GameObject ship)
    {
        playerRB2D = ship.GetComponent<Rigidbody2D>();
        playerTransform = ship.GetComponent<Transform>();
    }

    //Jerk -> just to lerp the acceleration
    public void Accelerates(float moveInput)
    {
        if (moveInput >= -0.001f && moveInput <= 0.001f)
        {
            accelerationTimer = 0.0f;
            currentAcceleration = 0.0f;
            trackAcceleration = playerRB2D.linearVelocity.magnitude;
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
        
        accelerationTimer += Time.deltaTime;
        currentAcceleration = Mathf.Lerp(trackAcceleration, currentShip.maxAcceleration, accelerationTimer/currentShip.timeUntilMaxAcceleration);
        playerRB2D.linearVelocity = playerTransform.up * moveInput * currentAcceleration;
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
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutoMatedTest : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputs;
    [SerializeField] private BossManager bossManager;
    [SerializeField] private PlayerManager playerManager;
    
    [HideInInspector] public bool isTesting = false;
    [HideInInspector] public float accelerationInput = 0f;
    [HideInInspector] public float turnInput = 0f;
    [HideInInspector] public bool missileInput = false;

    enum Sequence
    {
        LookToBossAndLaunchMissile,
        MoveAround
    }
    private float ltbDuration = 5f;
    private float ltbTimer = 0f;
    private float mvabDuration = 5f;
    private float mvabTimer = 0f;

    private Sequence currentSequence;

    private int moveCount = 0;
    private int theTurnWay = 0;

    private void Start()
    {
        currentSequence = Sequence.LookToBossAndLaunchMissile;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isTesting = !isTesting;
            InputEnabling(isTesting);
        }
        
        if (!isTesting) return;
        
        //input Sequencing
        switch (currentSequence)
        {
            case Sequence.LookToBossAndLaunchMissile:
                ResetAutoMoveAround();
                
                mvabTimer = 0f;
                ltbTimer += Time.deltaTime;
                if (ltbTimer >= ltbDuration)
                {
                    missileInput = true;
                    theTurnWay = UnityEngine.Random.Range(0,2);
                    currentSequence = Sequence.MoveAround;
                }
                else
                {
                    missileInput = false;
                }

                AutoLookToBoss();
                
                break;
            case Sequence.MoveAround:
                ResetLookToBoss();
                
                ltbTimer = 0f;
                mvabTimer += Time.deltaTime;
                if (mvabTimer >= mvabDuration)
                {
                    missileInput = true;
                    moveCount++;
                    currentSequence = Sequence.LookToBossAndLaunchMissile;
                }
                else
                {
                    missileInput = false;
                }

                AutoMoveAround();
                
                break;
        }
    }

    private void AutoLookToBoss()
    {
        Vector3 directionToTarget = (bossManager.transform.position - playerManager.transform.position).normalized;
        float dot = Vector2.Dot(playerManager.transform.up, directionToTarget);
        float signedAngle = Vector2.SignedAngle(transform.up, directionToTarget);
        Debug.Log(dot);

        if (dot > 0.99f)
        {
            turnInput = 0f;
        }
        else
        {
            if (signedAngle > 0f)
            {
                //counterclockwise
                turnInput = -1f;
            }
            else
            {
                //clockwise
                turnInput = 1f;
            }
        }
    }

    private void ResetLookToBoss()
    {
        turnInput = 0f;
    }

    private void AutoMoveAround()
    {
        int randMove = UnityEngine.Random.Range(0, 50);
        int randTurn = UnityEngine.Random.Range(0, 75);
        float[] lazy = new float[2];
        lazy[0] = 1f;
        lazy[1] = -1f;
        
        
        if (randMove != 0)
        {
            accelerationInput = 1f;
        }
        else
        {
            accelerationInput = 0f;
        }

        if (randTurn < 10)
        {
            turnInput = lazy[theTurnWay];
        }
        else
        {
            turnInput = 0f;
        }
    }

    private void ResetAutoMoveAround()
    {
        turnInput = 0f;
        accelerationInput = 0f;
    }

    private void InputEnabling(bool isTest)
    {
        if (isTest)
        {
            inputs.FindActionMap("Player").Disable();
        }
        else
        {
            inputs.FindActionMap("Player").Enable();
        }
    }
}

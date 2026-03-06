using System;
using System.Collections.Generic;
using Napadol.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : ActionListManager
{
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private InputActionReference shiftInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private List<ShipData> allShips;
    private ShipData _currentShipData;
    private GameObject _spawnedShip;

    private bool _isAutomatedTest;
    private bool _isHoldingShift = false;
    
    #region InputSetup

    private void OnEnable()
    {
        moveInput.action.Enable();
        shiftInput.action.Enable();
    }

    private void OnDisable()
    {
        moveInput.action.Disable();
        shiftInput.action.Disable();
    }

    #endregion
    
    private void Start()
    {
        AssignNewShip(0);
    }

    protected override void Update()
    {
        base.Update();
        this.gameObject.transform.position = _spawnedShip.transform.position;
        if (shiftInput.action.IsPressed())
        {
            playerMovement.DriftPulseCharge();
            _isHoldingShift = true;
        }
        else if(shiftInput.action.WasReleasedThisFrame())
        {
            playerMovement.DriftPulse();
            _isHoldingShift = false;
        }

        HandlingShipSwitch();
    }

    private void FixedUpdate()
    {
        if (!_isHoldingShift)
        {
            playerMovement.Accelerates(moveInput.action.ReadValue<Vector2>().y);
        }
        playerMovement.AngularAccelerates(moveInput.action.ReadValue<Vector2>().x);
    }

    private void HandlingShipSwitch()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            AssignNewShip(0);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            AssignNewShip(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            AssignNewShip(2);
        }
    }

    private void AssignNewShip(int i)
    {
        _currentShipData = allShips[i];
        if (_spawnedShip != null)
        {
            Destroy(_spawnedShip);
        }
        
        playerMovement.ResetMovement();
        
        _spawnedShip = Instantiate(allShips[i].shipSkinPrefab);
        playerMovement.SetUp(_spawnedShip, _currentShipData);
    }
}

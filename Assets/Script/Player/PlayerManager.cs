using System;
using System.Collections.Generic;
using Napadol.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : ActionListManager
{
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private List<ShipData> allShips;
    private ShipData _currentShipData;
    private GameObject _spawnedShip;

    private bool _isAutomatedTest;
    
    #region InputSetup

    private void OnEnable()
    {
        moveInput.action.Enable();
    }

    private void OnDisable()
    {
        moveInput.action.Disable();
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
    }

    private void FixedUpdate()
    {
        playerMovement.Accelerates(moveInput.action.ReadValue<Vector2>().y);
        playerMovement.AngularAccelerates(moveInput.action.ReadValue<Vector2>().x);
    }

    private void AssignNewShip(int i)
    {
        _currentShipData = allShips[i];
        if (_spawnedShip != null)
        {
            Destroy(_spawnedShip);
        }
        playerMovement.currentShip = _currentShipData;
        
        _spawnedShip = Instantiate(allShips[i].shipSkinPrefab);
        playerMovement.SetUp(_spawnedShip);
    }
}

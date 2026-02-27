using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : ActionListManager
{
    [SerializeField] private InputActionReference moveInput;

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
        
    }
}

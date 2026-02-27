using System;
using Napadol.Tools;
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
        actionList.AddAction(new MoveAction(this.gameObject, Vector3.zero, 0.5f).Easer(Easing.EaseOutBounce));
    }
}

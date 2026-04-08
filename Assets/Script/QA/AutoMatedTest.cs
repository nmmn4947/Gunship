using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutoMatedTest : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputs;
    
    [HideInInspector] public bool isTesting = false;
    [HideInInspector] public float accelerationInput = 0f;
    [HideInInspector] public float turnInput = 0f;
    [HideInInspector] public bool missileInput = false;
    
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isTesting = !isTesting;
            InputEnabling(isTesting);
        }
        
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

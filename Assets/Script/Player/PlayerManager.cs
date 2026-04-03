using System;
using System.Collections.Generic;
using Napadol.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : ActionListManager
{
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private InputActionReference shiftInput;
    [SerializeField] private InputActionReference spaceInput;
    [SerializeField] private InputActionReference enterInput;
    [SerializeField] private List<ShipData> allShips;
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject explosionParticle;

    public PlayerMovement playerMovement = new PlayerMovement();
    private PlayerChaingun playerChaingun = new PlayerChaingun();
    private PlayerMissiles playerMissiles;

    [HideInInspector] public ShipData _currentShipData;
    [HideInInspector] public GameObject _spawnedShip;

    private bool _isAutomatedTest;
    private bool _isHoldingShift = false;
    private Rigidbody2D rb2D;
    private CircleCollider2D playerCollider;
    private float regenTimer = 0f;

    #region InputSetup

    private void OnEnable()
    {
        moveInput.action.Enable();
        shiftInput.action.Enable();
        spaceInput.action.Enable();
        enterInput.action.Enable();
    }

    private void OnDisable()
    {
        moveInput.action.Disable();
        shiftInput.action.Disable();
        spaceInput.action.Disable();
        enterInput.action.Disable();
    }

    #endregion

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CircleCollider2D>();
        AssignNewShip(0);
    }

    protected override void Update()
    {
        base.Update();
        //this.gameObject.transform.position = _spawnedShip.transform.position;

        //movement
        if (shiftInput.action.IsPressed())
        {
            playerMovement.DriftPulseCharge();
            _isHoldingShift = true;
        }
        else if (shiftInput.action.WasReleasedThisFrame())
        {
            playerMovement.DriftPulse();
            _isHoldingShift = false;
        }

        //Combat
        playerChaingun.RampHandlingChainGun(spaceInput.action.IsPressed());
        playerChaingun.UpdateGun();
        if (enterInput.action.IsPressed())
        {
            playerMissiles.SpawnMissiles();
        }

        //regen
        if (!playerHealth.IsMaxHealth())
        {
            regenTimer += Time.unscaledDeltaTime;
        }

        if (regenTimer >= _currentShipData.regenCooldown)
        {
            playerHealth.Heal(_currentShipData.regenAmount);
            regenTimer = 0f;
        }

        UpdateLowHealthEffect();

        HandlingShipSwitch(); //switching ships
    }

    private void FixedUpdate()
    {
        if (!_isHoldingShift)
        {
            playerMovement.Accelerates(moveInput.action.ReadValue<Vector2>().y, playerHealth.isLowHealth);
        }

        playerMovement.AngularAccelerates(moveInput.action.ReadValue<Vector2>().x, playerHealth.isLowHealth);
        playerMovement.UpdateMovement();
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

        _spawnedShip = Instantiate(allShips[i].shipSkinPrefab, this.transform);
        playerMovement.SetUp(_spawnedShip, _currentShipData, rb2D);
        playerChaingun.SetUp(_spawnedShip, _currentShipData);

        playerMissiles = _spawnedShip.GetComponent<PlayerMissiles>();
        playerCollider.radius = _currentShipData.colliderRadius;

        playerHealth.maxHealth = (_currentShipData.maxHealth);
        playerHealth.FullHeal();
    }

    public void HurtVisual()
    {
        _spawnedShip.GetComponent<PlayerVisualManager>().HurtVisual();
    }

    private void UpdateLowHealthEffect()
    {
        Time.timeScale = Mathf.Lerp(0.3f, 1.0f, playerHealth.GetLowPercentage());
    }

    public void SpawnExplosion()
    {
        Instantiate(explosionParticle, this.transform.position, this.transform.rotation);
    }
}

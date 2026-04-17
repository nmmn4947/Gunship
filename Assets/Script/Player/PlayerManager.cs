using System;
using System.Collections.Generic;
using CardProject;
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
    [SerializeField] private float respawnTime;
    [SerializeField] private AutoMatedTest _autoMatedTest;

    public PlayerMovement playerMovement = new PlayerMovement();
    private PlayerChaingun playerChaingun = new PlayerChaingun();
    private PlayerMissiles playerMissiles;

    [HideInInspector] public ShipData _currentShipData;
    [HideInInspector] public GameObject _spawnedShip;

    private bool _isAutomatedTest;
    private bool _isHoldingShift = false;
    private bool _isDead = false;
    private bool _isMissileSpawn = false;
    private float respawnTimer = 0;
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
        TelemetryGenerator.instance.Logging += HandleTelemetry;
    }

    protected override void Update()
    {
        base.Update();

        //if dead
        if (_isDead)
        {
            respawnTimer += Time.unscaledDeltaTime;
            if (respawnTimer >= respawnTime)
            {
                respawnTimer = 0;
                rb2D.isKinematic = false;
                _isDead = false;
                AssignNewShip(UnityEngine.Random.Range(0, allShips.Count));
                playerMovement.ResetMovement();
                playerHealth.ResetHealth();
            }
            return;
        }

        if (!_autoMatedTest.isTesting)
        {
            PlayerUpdate();
        }
        else
        {
            AutomatedTestUpdate();
        }

        HandlingShipSwitch(); //switching ships
        
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
    }

    private void PlayerUpdate()
    {
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
            _isMissileSpawn = true;
            playerMissiles.SpawnMissiles();
        }
        else
        {
            _isMissileSpawn = false;
        }
    }

    private void AutomatedTestUpdate()
    {
        //Combat
        playerChaingun.RampHandlingChainGun(true);
        playerChaingun.UpdateGun();
        if (_autoMatedTest.missileInput)
        {
            _isMissileSpawn = true;
            playerMissiles.SpawnMissiles();
        }
        else
        {
            _isMissileSpawn = false;
        }
    }

    private void FixedUpdate()
    {
        if (_isDead) return;

        if (!_autoMatedTest.isTesting)
        {
            PlayerFixedUpdate();
        }
        else
        {
            AutomatedTestFixedUpdate();
        }
    }

    private void PlayerFixedUpdate()
    {
        if (!_isHoldingShift)
        {
            playerMovement.Accelerates(moveInput.action.ReadValue<Vector2>().y, playerHealth.isLowHealth);
        }
        
        playerMovement.AngularAccelerates(moveInput.action.ReadValue<Vector2>().x, playerHealth.isLowHealth);
        playerMovement.UpdateMovement();
    }
    
    private void AutomatedTestFixedUpdate()
    {
        playerMovement.Accelerates(_autoMatedTest.accelerationInput, playerHealth.isLowHealth);
        playerMovement.AngularAccelerates(_autoMatedTest.turnInput, playerHealth.isLowHealth);
        playerMovement.UpdateMovement();
    }

    public void ApplyKnockback(Vector2 f)
    {
        if (_currentShipData == allShips[0])
        {
            rb2D.AddForce(f, ForceMode2D.Impulse);
        }
        else if (_currentShipData == allShips[1])
        {
            rb2D.AddForce(f/2, ForceMode2D.Impulse);
        }
        else
        {
            rb2D.AddForce(f/5, ForceMode2D.Impulse);
        }
    }

    private void HandlingShipSwitch()
    {
        if (_isDead) return;
        
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
        
        rb2D.mass = _currentShipData.mass;

        string s = _spawnedShip.gameObject.name.Replace("(Clone)", "");
        AnnouncerManager.instance.SpawnSomething(s);
        
        TelemetryGenerator.instance.Log();
    }

    public void HurtVisual()
    {
        if (_isDead) return;
        _spawnedShip.GetComponent<PlayerVisualManager>().HurtVisual();
    }

    private void UpdateLowHealthEffect()
    {
        if (_isDead)
        {
            Time.timeScale = 1f;
            return;
        }
        Time.timeScale = Mathf.Lerp(0.3f, 1.0f, playerHealth.GetLowPercentage());
    }

    public void SpawnExplosion()
    {
        Instantiate(explosionParticle, this.transform.position, this.transform.rotation);
    }

    public void SetIsDead()
    {
        Destroy(_spawnedShip);
        Time.timeScale = 1;
        _isDead = true;
        this.transform.position = Vector2.zero;
        rb2D.isKinematic = true;
        playerMovement.ResetMovement();
        TelemetryGenerator.instance.Log();
    }

    public void HandleTelemetry()
    {
        TelemetryGenerator.instance.PlayerTelemetryData(playerHealth.currentHealth, this.transform.position, rb2D.linearVelocity.magnitude,
            this.transform.rotation.eulerAngles, playerChaingun.currentMultiplier, _isMissileSpawn);
    }
}

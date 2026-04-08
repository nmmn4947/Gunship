using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ShipData", menuName = "Scriptable Objects/ShipData")]
public class ShipData : ScriptableObject
{
    [Header("Movement")] 
    public GameObject shipSkinPrefab;
    public string shipName;
    public float maxSpeed;
    public float maxAcceleration;
    public float timeUntilMaxAcceleration;
    public float dragForce; //should be longer than acceleration time
    public float torque;
    public float pulseMaxPower;
    public float pulseChargeDuration;
    public float mass;

    [Header("Combat General")] 
    public GameObject bulletPrefab;
    public float colliderRadius;
    public int maxHealth;
    public float regenCooldown;
    public int regenAmount;

    
    [Header("Chain Gun")] 
    public float maxFireRate;
    public float maxRampUp;
    public float rampUpStepMultiplier;
    public float rampDownStepMultiplier;
    public float maxConeShotOffset = 0.5f;

    [Header("Missiles")] 
    public int missileCount;

}

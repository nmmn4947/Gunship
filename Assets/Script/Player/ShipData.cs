using UnityEngine;

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

    [Header("Combat General")] 
    public GameObject bulletPrefab;
    public int maxHealth;

    
    [Header("Chain Gun")] 
    public float maxFireTimeEachShot;
    public float rampUpSpeed;
    public float rampDownSpeed;

    [Header("Missiles")] 
    public int missileCount;

}

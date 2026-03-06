using UnityEngine;

[CreateAssetMenu(fileName = "ShipData", menuName = "Scriptable Objects/ShipData")]
public class ShipData : ScriptableObject
{
    public GameObject shipSkinPrefab;
    public string shipName;
    public float maxSpeed;
    public float maxAcceleration;
    public float timeUntilMaxAcceleration;
    public float dragForce; //should be longer than acceleration time
    public float maxAngularAcceleration;
    public float pulseMaxPower;
    public float pulseChargeDuration;
}

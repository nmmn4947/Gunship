using UnityEngine;

[CreateAssetMenu(fileName = "ShipData", menuName = "Scriptable Objects/ShipData")]
public class ShipData : ScriptableObject
{
    public GameObject shipSkinPrefab;
    public string shipName;
    public float maxAcceleration;
    public float timeUntilMaxAcceleration;
    public float maxAngularAcceleration;
    public float timeUntilMaxAngularAcceleration;
}

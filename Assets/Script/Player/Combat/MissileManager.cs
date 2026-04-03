using UnityEngine;

public class MissileManager : MonoBehaviour
{
    public GameObject missileExplosionPrefab;
    public void KillMissile()
    {
        Instantiate(missileExplosionPrefab, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}

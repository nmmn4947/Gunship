using System;
using UnityEngine;

public class BulletPool : GameObjectPool
{
    public static BulletPool instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
    }

    public void SpawnBullet(Vector3 position, Quaternion rotation)
    {
        GameObject spawned = GetAvailable();
        spawned.transform.position = position;
        spawned.transform.rotation = rotation;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissiles : MonoBehaviour
{
    [SerializeField] ShipData shipData;
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private float missileCooldown;
    private float timer = 0;
    private bool isOnCooldown = false;

    public void SpawnMissiles()
    {
        if (isOnCooldown)
        {
            return;
        }
        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
        }
        isOnCooldown = true;
    }

    private void Update()
    {
        if (isOnCooldown)
        {
            timer += Time.deltaTime;
            if (timer > missileCooldown)
            {
                isOnCooldown = false;
                timer = 0;
            }
        }
    }
}

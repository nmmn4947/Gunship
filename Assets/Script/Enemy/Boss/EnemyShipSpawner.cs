using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyShipSpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyShipPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    private int spawnPointRotater = 0;

    [SerializeField] private float cooldownTime;
    private float timer = 0f;

    private void Start()
    {
        SpawnEnemyShip();
        SpawnEnemyShip();
        SpawnEnemyShip();
        SpawnEnemyShip();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cooldownTime)
        {
            AnnouncerManager.instance.Announce("The Boss has Spawn Enemy-ships.");
            SpawnEnemyShip();
            SpawnEnemyShip();
            SpawnEnemyShip();
            SpawnEnemyShip();
            timer = 0;
        }
    }

    private void SpawnEnemyShip()
    {
        spawnPointRotater += 1;
        Transform spawnPoint = spawnPoints[spawnPointRotater%spawnPoints.Count];
        GameObject spwnd = Instantiate(enemyShipPrefab, spawnPoint.position, spawnPoint.rotation);
        spwnd.GetComponent<Rigidbody2D>().AddForce(spawnPoint.right * 10f, ForceMode2D.Impulse);
    }
}

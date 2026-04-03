using System;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float timeBetweenBullets;

    private float anchorTime;

    private void Start()
    {
        anchorTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > anchorTime + timeBetweenBullets)
        {
            anchorTime = Time.time;
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }
}

using System;
using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float timeBetweenBullets;
    [SerializeField] private bool isRand = false;
    [SerializeField] private Vector2 randomBetweenTwoConstant;

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
            if (isRand)
            {
                RandTime();
            }
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }

    private void RandTime()
    {
        timeBetweenBullets = UnityEngine.Random.Range(randomBetweenTwoConstant.x, randomBetweenTwoConstant.y);
    }
}

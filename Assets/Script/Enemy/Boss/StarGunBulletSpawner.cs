using System;
using UnityEngine;

public class StarGunBulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spwnPoint1;
    [SerializeField] private Transform spwnPoint2;
    [SerializeField] private Transform spwnPoint3;
    [SerializeField] private Transform spwnPoint4;
    
    [SerializeField] private float shootfrequencyTime;
    private float timerFreq = 0;
    [SerializeField] private float shootDuration;
    private float timerShoot = 0;
    [SerializeField] private float cooldownTime;
    private float timerCooldown = 0;

    private void Update()
    {
        timerCooldown += Time.deltaTime;
        if (timerCooldown >= cooldownTime)
        {
            timerShoot += Time.deltaTime;
            if (timerShoot >= shootDuration)
            {
                timerCooldown = 0f;
                timerShoot = 0f;
                timerFreq = 0f;
            }
            else
            {
                timerFreq += Time.deltaTime;
                if (timerFreq >= shootfrequencyTime)
                {
                    //shoot
                    ShootOnAllSpawnPoints();
                    timerFreq = 0;
                }
            }
        }
    }

    private void ShootOnAllSpawnPoints()
    {
        Instantiate(bulletPrefab, spwnPoint1.position, spwnPoint1.rotation);
        Instantiate(bulletPrefab, spwnPoint2.position, spwnPoint2.rotation);
        Instantiate(bulletPrefab, spwnPoint3.position, spwnPoint3.rotation);
        Instantiate(bulletPrefab, spwnPoint4.position, spwnPoint4.rotation);
    }
}

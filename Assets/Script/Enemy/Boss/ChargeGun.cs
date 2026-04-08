using System;
using UnityEngine;

public class ChargeGun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spwnPoint;
    [SerializeField] private float maxScaleBulletMultiplier;
    [SerializeField] private Transform gunSkinTransform;
    
    [SerializeField] private float chargeDuration;
    [HideInInspector] public float timerCharge = 0;
    [SerializeField] private float cooldownDuration;
    [HideInInspector] public float timerCooldown = 0;
    
    GameObject spwnd = null;
    BulletManager bulletManager = null;
    
    private bool once = false;
    private Transform player;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerManager>().transform;
    }

    private void Update()
    {
        timerCooldown += Time.deltaTime;
        if (timerCooldown >= cooldownDuration)
        {
            if (!once)
            {
                spwnd = Instantiate(bulletPrefab, spwnPoint);
                Destroy(spwnd.GetComponent<Rigidbody2D>());
                bulletManager = spwnd.GetComponent<BulletManager>();
                if (bulletManager == null)
                {
                    Debug.LogError("BulletManager not found");
                }
                once = true;
            }
            spwnd.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(maxScaleBulletMultiplier, maxScaleBulletMultiplier, 1), timerCharge/chargeDuration);
            
            timerCharge += Time.deltaTime;
            if (timerCharge >= chargeDuration)
            {
                Rigidbody2D rb = spwnd.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0;
                bulletManager.FireBullet();
                spwnd.transform.parent = null;
                spwnd = null;
                bulletManager = null;
                timerCharge = 0;
                timerCooldown = 0;
                once = false;
            }
        }
        
        Vector2 direction = player.position - gunSkinTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        gunSkinTransform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}

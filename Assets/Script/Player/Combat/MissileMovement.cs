using System;
using System.Collections.Generic;
using UnityEngine;

public class MissileMovement : MonoBehaviour
{
    [SerializeField] private float _jerk;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAngularSpeed;
    [SerializeField] private bool isBoss;
    
    private Transform target;
    private Rigidbody2D rb2D;
    private MissileManager missileManager;
    
    private float angularSpeedMultiplier = 1;
    private float currentAcceleration = 0;
    private float currentSpeed = 0;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if (!isBoss)
        {
            FindTarget();
        }
        else
        {
            target = FindAnyObjectByType<PlayerManager>().transform;
        }
        missileManager = GetComponent<MissileManager>();
        
    }

    private void Update()
    {
        if (!isBoss)
        {
            if (!target.gameObject.activeInHierarchy || target == null)
            {
                FindTarget();
            }
            BossManager boss = target.GetComponent<BossManager>();
            if (boss != null)
            {
                if (boss.isDead)
                {
                    target = null;
                    FindTarget();
                }
            }
        }
        else
        {
            
        }



    }

    private void FindTarget()
    {
        List<Transform> targets = new List<Transform>();
        BossManager boss = FindAnyObjectByType<BossManager>(FindObjectsInactive.Exclude);
        Transform bossPos = boss.transform;
        if (!boss.isDead)
        {
            targets.Add(bossPos);
        }
        
        EnemyManager[] enemyships = FindObjectsByType<EnemyManager>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var enemy in enemyships)
        {
            targets.Add(enemy.gameObject.transform);
        }

        target = null;
        
        float dist = float.MaxValue;
        foreach (var enemy in targets)
        {
            float currDist = Vector3.Distance(this.transform.position, enemy.transform.position);
            if (dist > currDist)
            {
                target = enemy.transform;
                dist = currDist;
            }
        }

        if (target == null)
        {
            Debug.Log("Target is null");
            missileManager.KillMissile();
        }
    }
    
    private void Accelerates()
    {
        if (Mathf.Abs(currentAcceleration) < maxAcceleration)
        {
            currentAcceleration += _jerk * Time.fixedDeltaTime;
        }
    }

    private void Rotates()
    {
        Vector2 dir = target.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        float angleDiff = Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle);

        // Rotate directly instead of AddTorque
        float rotation = Mathf.Clamp(angleDiff, -maxAngularSpeed, maxAngularSpeed) * Time.fixedDeltaTime * angularSpeedMultiplier;
        transform.Rotate(0, 0, rotation);
        if (isBoss)
        {
            
        }
        else
        {
            angularSpeedMultiplier += Time.fixedDeltaTime;
        }
    }
    
    private void FixedUpdate()
    {
        Accelerates();
        Rotates();
        if (currentSpeed <= maxSpeed)
        {
            currentSpeed += currentAcceleration * Time.fixedDeltaTime;
        }

        if (!isBoss)
        {
            rb2D.linearVelocity = this.transform.up * currentSpeed;
        }
        else
        {
            rb2D.AddForce(this.transform.up * currentAcceleration, ForceMode2D.Force);
        }
    }
}

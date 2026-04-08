using System;
using System.Collections;
using CardProject;
using UnityEngine;
using Napadol.Tools;

public class BossManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPart;
    [SerializeField] private GameObject backPart;
    [SerializeField] private GameObject leftPart;
    [SerializeField] private GameObject rightPart;
    [SerializeField] private GameObject player;
    [SerializeField] private float deadBulletTime;
    
    [HideInInspector] public bool isDead = false;
    private MissileMovement mm;

    [SerializeField] private Health bossHealth;
    private Rigidbody2D rb2D;
    private bool backsIsAttached = true;
    [SerializeField] private EnemyShipSpawner enmshpspwnr; 
    private bool leftIsAttached = true;
    [SerializeField] private ChargeGun cg;
    private bool rightIsAttached = true;
    [SerializeField] private StarGunBulletSpawner strgnbltspwnr;
    private float timer = 0f;

    private void Start()
    {
        mm = GetComponent<MissileMovement>();
        rb2D = GetComponent<Rigidbody2D>();
        TelemetryGenerator.instance.Logging += HandleTelemetry;
    }

    public void DisconnectParts(GameObject part)
    {
        mm.AddMaxAngularSpeed(4);
        mm.AddAcceleration(2);
        mm.AddMaxSpeed(4);
        StartCoroutine(RunDisconnectParts(part));
        switch (part.name)
        {
            case "Backs":
                backsIsAttached = false;
                break;
            case "Left":
                leftIsAttached = false;
                break;
            case "Right":
                rightIsAttached = false;
                break;
        }
        TelemetryGenerator.instance.Log();
    }

    private IEnumerator RunDisconnectParts(GameObject part)
    {
        if (part.transform.parent == null)
        {
            yield break;
        }
        Vector3 originalPosition = part.GetComponent<BossHurtVisual>().offsetKnock;
        part.transform.parent = null;
        Rigidbody2D rb;
        if(part.GetComponent<Rigidbody2D>() == null){rb = part.AddComponent<Rigidbody2D>();}
        else
        {
            rb = part.GetComponent<Rigidbody2D>();
        }

        rb.gravityScale = 0;
        rb.linearDamping = 2f;
        yield return new WaitForFixedUpdate();
        rb.AddForce(originalPosition * 5f, ForceMode2D.Impulse);
    }

    public void BossDiedTimeSlowDown()
    {
        isDead = true;
        TelemetryGenerator.instance.Log();
        StartCoroutine(SlowTimeOnDeath());
    }

    private IEnumerator SlowTimeOnDeath()
    {
        while (timer <= deadBulletTime)
        {
            timer += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(0.05f, 1f, Easing.EaseInExpo(timer / deadBulletTime));
            yield return null;
        }
        
        Time.timeScale = 1f;
    }

    private void HandleTelemetry()
    {
        TelemetryGenerator.instance.BossTelemetryData(bossHealth.currentHealth, this.transform.position,rb2D.linearVelocity.magnitude, 
            backsIsAttached, enmshpspwnr.timer,
            leftIsAttached, cg.timerCharge, cg.timerCooldown,
            rightIsAttached, strgnbltspwnr.timerCooldown);
    }
}

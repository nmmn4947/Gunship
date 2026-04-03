using System;
using UnityEngine;
using UnityEngine.Events;

public class SimpleDamager : MonoBehaviour
{
    public int damage;
    public Health.CombatTeam combatTeam;
    public UnityEvent hitSomething;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Health health = other.GetComponent<Health>();
        DamageRedirect damageRedirect = other.GetComponent<DamageRedirect>();
        if (damageRedirect != null && health == null)
        {
            health = damageRedirect.redirectTo;
        }
        
        if (health != null)
        {
            if (health.combatTeam != combatTeam)
            {
                health.TakeDamage(damage);
                //apply knockback
                Vector2 dir = (Vector2)other.transform.position - (Vector2)this.transform.position;
                switch (health.combatTeam)
                {
                    case Health.CombatTeam.Player:
                        health.GetComponent<PlayerManager>().playerMovement.ApplyKnockback(dir * 10f);
                        break;
                    case Health.CombatTeam.Enemy:
                        other.GetComponent<Rigidbody2D>().AddForce(dir * 2f, ForceMode2D.Impulse);
                        break;
                    case Health.CombatTeam.Boss:
                        break;
                }
                hitSomething?.Invoke();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        DamageRedirect damageRedirect = other.gameObject.GetComponent<DamageRedirect>();
        if (damageRedirect != null && health == null)
        {
            health = damageRedirect.redirectTo;
        }
        
        if (health != null)
        {
            if (health.combatTeam != combatTeam)
            {
                health.TakeDamage(damage);
                //apply knockback
                Vector2 dir = (Vector2)other.transform.position - (Vector2)this.transform.position;
                switch (health.combatTeam)
                {
                    case Health.CombatTeam.Player:
                        health.GetComponent<PlayerManager>().playerMovement.ApplyKnockback(dir * 10f);
                        break;
                    case Health.CombatTeam.Enemy:
                        other.gameObject.GetComponent<Rigidbody2D>().AddForce(dir * 2f, ForceMode2D.Impulse);
                        break;
                }
                hitSomething?.Invoke();
            }
        }
    }
}

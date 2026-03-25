using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    
    public enum CombatTeam
    {
        Player,
        Enemy
    }
    public CombatTeam combatTeam;
    public UnityEvent onDeath;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void FullHeal()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
        }
    }
}

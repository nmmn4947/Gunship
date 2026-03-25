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
    
    public UnityEvent onDamaged;
    public UnityEvent onDeath;
    
    [HideInInspector] public bool isLowHealth = false;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth/maxHealth < 0.25f)
        {
            isLowHealth = true;
        }
        else
        {
            isLowHealth = false;
        }
    }

    public void FullHeal()
    {
        currentHealth = maxHealth;
    }

    public void Heal(int amount)
    {
        if (currentHealth + amount >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
        }
        onDamaged?.Invoke();
    }
}

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
        Enemy,
        Boss
    }
    public CombatTeam combatTeam;
    
    public UnityEvent onDamaged;
    public UnityEvent onDeath;
    
    [HideInInspector] public bool isLowHealth = false;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private bool announce = false;
    
    private bool isDead = false;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public float GetLowPercentage()
    {
        return ((float)currentHealth / maxHealth)/0.3f;
    }
    
    private void Update()
    {
        if ((float)currentHealth/maxHealth < 0.3f)
        {
            isLowHealth = true;
        }
        else
        {
            isLowHealth = false;
        }
        smokeEffect.SetActive(isLowHealth);
    }

    public bool IsMaxHealth()
    {
        return currentHealth == maxHealth;
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
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            if(announce) AnnouncerManager.instance.Announce(this.gameObject.name + " is Dead.");
            
            onDeath?.Invoke();
        }
        onDamaged?.Invoke();
    }
}

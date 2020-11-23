using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class PlayerLifeManager : NetworkedBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int damage;

    public HealthBarPlayer healthBar;

    void Start()
    {
        if(IsServer)
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    private void OnCollisionStay(Collision col)
    {
        if (IsServer)
        {
            if (col.gameObject.GetComponent<WeaponManager>())
            {
                if(IsOwner && col.gameObject.GetComponent<WeaponManager>().getPlayer() == 1)
                    TakeDamage(damage);
                if (!IsOwner && col.gameObject.GetComponent<WeaponManager>().getPlayer() == 1)
                    TakeDamage(damage);
            }
        }
    }

    void TakeDamage(int damages)
    {
        currentHealth -= damages;
        
        healthBar.SetHealth(currentHealth);
    }
}
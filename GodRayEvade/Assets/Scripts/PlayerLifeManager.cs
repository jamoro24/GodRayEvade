using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkedVar;

public class PlayerLifeManager : NetworkedBehaviour
{
    public int maxHealth;
    public int damage;
    public NetworkedVar<int> currentHealth;

    public HealthBarPlayer healthBar;

    void Start()
    {
        if(IsServer)
        {
            currentHealth.Value = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    private void OnCollisionStay(Collision col)
    {
        if (IsServer)
        {
            if (col.gameObject.GetComponent<WeaponManager>())
            {
                if(IsOwner && !col.gameObject.GetComponent<WeaponManager>().IsOwner)
                    TakeDamage(damage);
                if (!IsOwner && col.gameObject.GetComponent<WeaponManager>().IsOwner)
                    TakeDamage(damage);
            }
        }
    }

    void TakeDamage(int damages)
    {
        currentHealth.Value -= damages;
        
        
    }

    void Update()
    {
        healthBar.SetHealth(currentHealth.Value);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int damage;

    public HealthBarPlayer healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void OnCollisionStay(Collision col)
    {
        //TODO
        /**if (col.gameObject.name == "RayPlayer2")
        {
            TakeDamage(damage);
        }**/
    }

    void TakeDamage(int damages)
    {
        currentHealth -= damages;
        
        healthBar.SetHealth(currentHealth);
    }
}
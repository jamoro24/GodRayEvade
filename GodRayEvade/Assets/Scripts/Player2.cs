using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int damage;

    public HealthBarPlayer2 healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

   private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.name == "RayPlayer1")
        {
            //StartCoroutine (WaitForSeconds());
            TakeDamage(damage);
        }
    }
    
    /*IEnumerator WaitForSeconds()
    {
        yield return new WaitForSecondsRealtime (0.5f);
    }*/

    void TakeDamage(int damages)
    {
        currentHealth -= damages;
        
        healthBar.SetHealth(currentHealth);
    }


   
}

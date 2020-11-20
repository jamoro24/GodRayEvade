using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPlayer1 : MonoBehaviour
{
    public Slider slider1;
    
    public void SetMaxHealth(int health)
    {
        slider1.maxValue = health;
        slider1.value = health;
    }

    public void SetHealth(int health)
    {
        slider1.value = health;
    }
}

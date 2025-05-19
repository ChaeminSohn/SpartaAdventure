using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthChangeEvent 
{
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }
    public PlayerHealthChangeEvent(float currentHealth, float maxHealth) 
    {
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }
}

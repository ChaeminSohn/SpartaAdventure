using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaminaChangeEvent 
{
    public float CurrentStamina { get; private set; }
    public float MaxStamina { get; private set; }
    public PlayerStaminaChangeEvent(float currentStamina, float maxStamina) 
    {
        CurrentStamina = currentStamina;
        MaxStamina = maxStamina;
    }
}

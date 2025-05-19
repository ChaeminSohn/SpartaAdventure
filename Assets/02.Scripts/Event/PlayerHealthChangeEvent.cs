using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthChangeEvent : EventBase
{
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public PlayerHealthChangeEvent(int currentHealth, int maxHealth) 
        : base(GameEventType.PlayerHealthChanged)
    {
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

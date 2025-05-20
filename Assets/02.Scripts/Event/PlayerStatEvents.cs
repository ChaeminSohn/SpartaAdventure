using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealEvent
{
    public float healValue;

    public PlayerHealEvent(float value)
    {
        healValue = value;
    }
}

public class PlayerSpeedUpEvent
{
    public float speedUpValue;
    public float speedUpDuration;

    public PlayerSpeedUpEvent(float value, float duration) 
    {
        speedUpValue = value;
        speedUpDuration = duration;
    }
}

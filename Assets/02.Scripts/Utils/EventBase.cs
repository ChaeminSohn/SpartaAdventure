using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventBase : EventArgs
{
    public GameEventType Type { get; protected set; }

    public EventBase(GameEventType type)
    {
        Type = type;
    }
}

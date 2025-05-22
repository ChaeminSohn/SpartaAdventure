using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewChangeEvent
{
    public bool isFirstPersonView { get; private set; }

    public PlayerViewChangeEvent(bool isFirstPersonView)
    {
        this.isFirstPersonView = isFirstPersonView;
    }
}

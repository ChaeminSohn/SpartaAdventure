using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionEvent 
{
    public ItemData itemData;

    public PlayerInteractionEvent(ItemData itemData)
    {
        this.itemData = itemData;
    }
}

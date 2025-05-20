using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovementCtrl movementCtrl { get; private set; }
    public PlayerStat stat { get; private set; }

    private void Awake()
    {
        movementCtrl = GetComponent<PlayerMovementCtrl>();
        stat = GetComponent<PlayerStat>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public float Health { get; private set; }
    public float Stamina { get; private set; }

    private PlayerStat stat;

    private void Start()
    {
        stat = GetComponent<PlayerStat>();
        if(stat == null)
        {
            Debug.LogWarning(this.name + ": Player Stat Component Not Found");
            return;
        }
    }

    public void OnDamage(float damage)
    {

    }
}

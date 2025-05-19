using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Health,
    MaxHealth,
    Stamina,
    MaxStamina,
    Speed,
    JumpPower
}
public class PlayerStat : MonoBehaviour
{
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }
    public float CurrentStamina { get; private set; }
    public float MaxStamina { get; private set; }
    public float Speed { get; private set; }

    public void GetDamage(float amount)
    {
        if (amount <= 0) return;

        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        Debug.Log($"플레이어가 {amount} 의 데미지를 입었습니다. 현재 체력 : {CurrentHealth} / {MaxHealth}");
        //이벤트 실행

        if(CurrentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("플레잉어가 사망했습니다.");
        //사망 관련 로직
    }
}

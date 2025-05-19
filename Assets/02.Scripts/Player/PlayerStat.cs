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

        Debug.Log($"�÷��̾ {amount} �� �������� �Ծ����ϴ�. ���� ü�� : {CurrentHealth} / {MaxHealth}");
        //�̺�Ʈ ����

        if(CurrentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("�÷��׾ ����߽��ϴ�.");
        //��� ���� ����
    }
}

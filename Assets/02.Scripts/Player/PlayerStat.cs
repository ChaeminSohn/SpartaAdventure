using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    [Header("Base Stats")]
    [SerializeField, Range(0, 200f)] private float baseHealth = 100f;
    [SerializeField, Range(0, 200f)] private float baseStamina = 100f;
    [SerializeField, Range(0f, 10f)] private float baseSpeed = 5f;
    [SerializeField, Range(0f, 100f)] private float baseJumpPower = 80f;
    
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }
    public float CurrentStamina { get; private set; }
    public float MaxStamina { get; private set; }
    public float Speed { get; private set; }
    public float JumpPower { get; private set; }

    private void Start()
    {
        MaxHealth = baseHealth;
        CurrentHealth = MaxHealth;
        MaxStamina = baseStamina;
        CurrentStamina = MaxStamina;
        Speed = baseSpeed;
        JumpPower = baseJumpPower;

        StartCoroutine(TestCoroutine());
    }
    public void GetDamage(float amount)
    {
        if (amount <= 0) return;

        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        Debug.Log($"�÷��̾ {amount} �� �������� �Ծ����ϴ�. ���� ü�� : {CurrentHealth} / {MaxHealth}");
        //�̺�Ʈ ����
        EventBus.Raise(new PlayerHealthChangeEvent(CurrentHealth, MaxHealth));

        if (CurrentHealth == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0) return;

        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        Debug.Log($"�÷��̾ {amount} ��ŭ�� ü���� ȸ���߽��ϴ�.. ���� ü�� : {CurrentHealth} / {MaxHealth}");

        EventBus.Raise(new PlayerHealthChangeEvent(CurrentHealth, MaxHealth));
    }

    public bool UseStamina(float amount)
    {
        if (amount <= 0) return false;

        if(CurrentStamina >= amount)
        {
            CurrentStamina -= amount;
            //�̺�Ʈ ����
            EventBus.Raise(new PlayerStaminaChangeEvent(CurrentStamina, MaxStamina));
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Die()
    {
        Debug.Log("�÷��׾ ����߽��ϴ�.");
        //��� ���� ����
    }

   IEnumerator TestCoroutine()
    {
        while (true)
        {
            GetDamage(1f);
            yield return new WaitForSeconds(1f);
        }
    }
}

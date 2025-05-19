using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider healthSlider;

    //체력이 감소하면 체력 바 색이 변하거나 깜빡거리는 옵션 추가? 예정

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerHealthChangeEvent>(PlayerHealthChangeHandler);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<PlayerHealthChangeEvent>(PlayerHealthChangeHandler);
    }

    //PlayerHealthChangeEvent 발생 시 호출될 핸들러
    private void PlayerHealthChangeHandler(PlayerHealthChangeEvent args)
    {
        UpdateUI(args.CurrentHealth, args.MaxHealth);
    }

    private void UpdateUI(float currentHealth, float maxHealth)
    {
        if(healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
    }
}

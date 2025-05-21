using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider staminaSlider;


    private void OnEnable()
    {
        EventBus.Subscribe<PlayerStaminaChangeEvent>(PlayerStaminaChangeHandler);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<PlayerStaminaChangeEvent>(PlayerStaminaChangeHandler);
    }

    private void PlayerStaminaChangeHandler(PlayerStaminaChangeEvent args)
    {
        UpdateUI(args.CurrentStamina,  args.MaxStamina);
    }

    private void UpdateUI(float currentStamina, float maxStamina)
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina / maxStamina;
        }
    }
}

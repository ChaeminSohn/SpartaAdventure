using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider healthSlider;

    //ü���� �����ϸ� ü�� �� ���� ���ϰų� �����Ÿ��� �ɼ� �߰�? ����

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerHealthChangeEvent>(PlayerHealthChangeHandler);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<PlayerHealthChangeEvent>(PlayerHealthChangeHandler);
    }

    //PlayerHealthChangeEvent �߻� �� ȣ��� �ڵ鷯
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemEffectType
{
    None,
    Heal,
    StaminaRestore,
    SpeedBoost,
    JumpBoost
}
public class UsableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemDataSO;
    public ItemType ItemType => ItemType.Usable;

    public ItemData ItemData => itemDataSO;

    public string GetInteractPrompt()
    {
        return "아이템 사용";
    }

    public void OnInteract()
    {
        switch (ItemData.effectType)
        {
            case ItemEffectType.Heal:
                EventBus.Raise(new PlayerHealEvent(ItemData.effectValue));
                break;
            case ItemEffectType.StaminaRestore:
                EventBus.Raise(new PlayerStaminaRestoreEvent(ItemData.effectValue));
                break;
            case ItemEffectType.SpeedBoost:
                EventBus.Raise(new PlayerSpeedUpEvent(ItemData.effectValue, itemDataSO.effectDuration));
                break;
            case ItemEffectType.JumpBoost:
                EventBus.Raise(new PlayerJumpBoostEvent(ItemData.effectValue, ItemData.effectDuration));
                break;
            default:
                break;
        }
        EventBus.Raise(new PlayerInteractedWithObjectEvent(this));
        Destroy(gameObject);
    }
}

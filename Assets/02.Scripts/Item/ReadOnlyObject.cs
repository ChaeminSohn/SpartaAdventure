using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadOnlyObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemDataSO;
    public ItemType ItemType => ItemType.Readonly;
    public ItemData ItemData =>  itemDataSO;
    public string GetInteractPrompt()
    {
        return "살펴보기";
    }

    public void OnInteract()
    {
        EventBus.Raise(new PlayerInteractedWithObjectEvent(this));
    }
}

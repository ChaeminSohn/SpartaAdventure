using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IInteractable
{
    public ItemType ItemType { get;}
    public ItemData ItemData { get;}
    public string GetInteractPrompt();
    public void OnInteract();
}
public class PickUpObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemDataSO;
    public ItemType ItemType => ItemType.Equipable;
    public ItemData ItemData => itemDataSO;
    public string GetInteractPrompt()
    {
        return "아이템 줍기";
    }

    public void OnInteract()
    {
        
    }
}

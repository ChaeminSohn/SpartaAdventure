using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,      //장비 
    Usable,         //소비    
    Resource,       //기타
    Readonly        //읽기 전용(팻말 등)
}


[CreateAssetMenu(fileName = "NewItemData", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    public ItemType type;
    public string displayName;
    [TextArea(3,10)]  public string description;

    [Header("능력치")]
    public float maxHealth;
    public float maxStamina;
    public float speed;
    public float jumpPower;

    [Header("기타 설정")]
    public bool isStackable;
    public int maxStackSize;

    [Header("소비 아이템 설정")]
    public ItemEffectType effectType;
    public float effectValue;
    public float effectDuration;

    [SerializeField]
    private int uniqueID;
    public int UniqueID => uniqueID;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,      //��� 
    Usable,         //�Һ�    
    Resource,       //��Ÿ
    Readonly        //�б� ����(�ָ� ��)
}


[CreateAssetMenu(fileName = "NewItemData", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    [Header("�⺻ ����")]
    public ItemType type;
    public string displayName;
    [TextArea(3,10)]  public string description;

    [Header("�ɷ�ġ")]
    public float maxHealth;
    public float maxStamina;
    public float speed;
    public float jumpPower;

    [Header("��Ÿ ����")]
    public bool isStackable;
    public int maxStackSize;

    [Header("�Һ� ������ ����")]
    public ItemEffectType effectType;
    public float effectValue;
    public float effectDuration;

    [SerializeField]
    private int uniqueID;
    public int UniqueID => uniqueID;
}

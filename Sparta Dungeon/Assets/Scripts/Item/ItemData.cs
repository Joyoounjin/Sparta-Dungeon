using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------ 아이템 데이터(속성) ------
public enum ItemType
{
    Resource, // 자원
    Equipable, // 장착
    Consumable // 소비 
}

public enum ConsumableType // 소비형 아이템을 다시 세분화
{
    HP,
    Stamina
}

[System.Serializable]
public class ItemDataConsumable 
{
    public ConsumableType type;
    public float value; // 소비값 
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] // 기본 정보 
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")] // 여러 개 소지 가능?
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")] // 소비형 아이템의 효과를 저장
    public ItemDataConsumable[] consumables;

    [Header("Equip")] // 장착 
    public GameObject equipPrefab;
}

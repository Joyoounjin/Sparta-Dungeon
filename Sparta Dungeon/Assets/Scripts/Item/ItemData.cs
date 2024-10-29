using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------ ������ ������(�Ӽ�) ------
public enum ItemType
{
    Resource, // �ڿ�
    Equipable, // ����
    Consumable // �Һ� 
}

public enum ConsumableType // �Һ��� �������� �ٽ� ����ȭ
{
    HP,
    Stamina
}

[System.Serializable]
public class ItemDataConsumable 
{
    public ConsumableType type;
    public float value; // �Һ� 
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] // �⺻ ���� 
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")] // ���� �� ���� ����?
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")] // �Һ��� �������� ȿ���� ����
    public ItemDataConsumable[] consumables;

    [Header("Equip")] // ���� 
    public GameObject equipPrefab;
}

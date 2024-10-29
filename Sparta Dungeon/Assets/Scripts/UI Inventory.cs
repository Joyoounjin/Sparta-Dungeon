using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;

    // UI Inventory 아이템 정보 표시
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private int curEquipIndex;

    private PlayerController controller;
    private PlayerCondition condition;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += Toggle;
        //CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            //slots[i].Clear();
        }

        ClearSelectedItemWindow();
    }

    public void Toggle() // UI on/off
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack) // 중복 가능? 
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                //UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot(); // 아니라면 빈 슬롯 

        if (emptySlot != null) 
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            //UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    //public void UpdateUI()
    //{
    //    for (int i = 0; i < slots.Length; i++)
    //    {
    //        if (slots[i].item != null)
    //        {
    //            slots[i].Set();
    //        }
    //        else
    //        {
    //            slots[i].Clear();
    //        }
    //    }
    //}

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    //public void SelectItem(int index)
    //{
    //    if (slots[index].item == null) return;

    //    selectedItem = slots[index];
    //    selectedItemIndex = index;

    //    selectedItemName.text = selectedItem.item.displayName;
    //    selectedItemDescription.text = selectedItem.item.description;

    //    selectedItemStatName.text = string.Empty;
    //    selectedItemStatValue.text = string.Empty;

    //    for (int i = 0; i < selectedItem.item.consumables.Length; i++)
    //    {
    //        selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
    //        selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
    //    }

    //    useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
    //    equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].equipped);
    //    unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].equipped);
    //    dropButton.SetActive(true);
    //}

    void ClearSelectedItemWindow()
    {
        selectedItem = null; // 선택한 아이템이 없다면

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    //public void OnUseButton()
    //{
    //    if (selectedItem.item.type == ItemType.Consumable)
    //    {
    //        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
    //        {
    //            switch (selectedItem.item.consumables[i].type)
    //            {
    //                case ConsumableType.HP:
    //                    condition.Heal(selectedItem.item.consumables[i].value); break;
    //            }
    //        }

    //        RemoveSelctedItem();
    //    }
    //}

    //public void OnDropButton()
    //{
    //    ThrowItem(selectedItem.item);
    //    RemoveSelctedItem();
    //}

    //void RemoveSelctedItem()
    //{
    //    selectedItem.quantity--;

    //    if (selectedItem.quantity <= 0)
    //    {
    //        //if (slots[selectedItemIndex].equipped)
    //        //{
    //        //    UnEquip(selectedItemIndex);
    //        //}

    //        selectedItem.item = null;
    //        ClearSelectedItemWindow();
    //    }

    //    UpdateUI();
    //}

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

// ------ UIIventory Manager ------
// ------ UIIventory 오브젝트와 연결 -------
public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots; // 배열로 저장 

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;

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

    private void OnEnable()
    {
        Debug.Log("오류");
    } // ------ error check ------

    void Start() // ------ 초기화 ------
    {
        // 싱글턴 Instance 캐싱 가능 
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // event 구독
        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);

        // slotPanel 패널의 자식 오브젝트를 Index(0~n)로 저장
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>(); // 런타임 내에서만, 그래서 GetComponent 사용 가능
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        ClearSelectedItemWindow();
    }

    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    } // ------ Tab 키를 눌렀을 때 ------
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    } 

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack) // 나무가 하나 있으면 기존의 나무에 숫자를 올려준다. 
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot(); // 아니라면 빈 슬롯 

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    } // ------ data에 저장한 아이템을 인벤토리에 넣기 ------

    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    } // ------ 아이템 버리기 ------

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();  
            }
            else
            {
                slots[i].Clear();
            }
        }
    } // ------ 아이템 선택 시 UI 업데이트 ------

    ItemSlot GetItemStack(ItemData data) // 숫자 올리기
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

    ItemSlot GetEmptySlot() // 빈 슬롯 가져오기 
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

    public void SelectItem(int index) // 아이템 클릭 시 
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    void ClearSelectedItemWindow() // UI 비우기
    {
        selectedItem = null; 

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch (selectedItem.item.consumables[i].type)
                {
                    case ConsumableType.HP:
                        condition.Heal(selectedItem.item.consumables[i].value); break;
                    case ConsumableType.Stamina:
                        condition.Heal(selectedItem.item.consumables[i].value); break;
                }
            }

            RemoveSelctedItem();
        }
    } // 사용하기 클릭 시

    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelctedItem();
    } // 버리기 클릭 시

    void RemoveSelctedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0)
        {
            //if (slots[selectedItemIndex].equipped)
            //{
            //    UnEquip(selectedItemIndex);
            //}

            selectedItem.item = null;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    } // 선택한 아이템 비우기

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }
}
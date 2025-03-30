using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EInventory
{
    Main,
    Quick,
    HeadArmor,
    BodyArmor,
    FootArmor,
}

public class InventoryManager : MonoSingleton<InventoryManager>
{
    private Dictionary<EInventory, Inventory> _inventoryDictionary;
    private Slot _dragSlot;
    public Slot DragSlot
    {
        get => _dragSlot;
        set => _dragSlot = value;
    }
    private Slot _pointerSlot;
    public Slot PointerSlot
    {
        get => _pointerSlot;
        set => _pointerSlot = value;
    }

    public Action<int, Item> OnQuickSlotChangeEvent;

    private void Awake()
    {
        _inventoryDictionary = new Dictionary<EInventory, Inventory>();
        FindObjectsByType<Inventory>(FindObjectsSortMode.None).ToList()
            .ForEach(inventory =>
            {
                inventory.Init();
                _inventoryDictionary.Add(inventory.inventoryEnum, inventory);
            });
    }

    public Slot GetSlot(EInventory inventory, int index)
        => _inventoryDictionary[inventory].GetSlot(index);
    public Slot GetSlot(EInventory inventory, Vector2Int pos)
        => _inventoryDictionary[inventory].GetSlot(pos);

    public Item AddItem(EInventory inventory, Item item)
    {
        return _inventoryDictionary[inventory].AddItem(item);
    }
    public int AddItem(EInventory inventory, ItemSO itemSO, int amount = 1, int durability = -1)
    {
        int over = 0;
        while (amount > 0)
        {
            Item item = new Item();
            item.itemSO = itemSO;
            item.SetAmount(amount);
            item.SetDurability(durability == -1 ? itemSO.durability : durability);
            amount -= item.Amount;
            Item overItem = _inventoryDictionary[inventory].AddItem(item);
            if (overItem != null)
                over += overItem.Amount;
        }
        return over;
    }
    public Item RemoveItem(EInventory inventory, Item item)
    {
        return _inventoryDictionary[inventory].RemoveItem(item);
    }
    public Item RemoveItem(EInventory inventory, ItemSO itemSO, int amount = 1)
    {
        Item item = new Item();
        item.itemSO = itemSO;
        item.SetAmount(amount);
        return _inventoryDictionary[inventory].RemoveItem(item);
    }
    public int GetItemAmount(EInventory inventory, EItemID eItemName)
    {
        return _inventoryDictionary[inventory].GetItemAmount(eItemName);
    }
    public void AddChangeListener(EInventory inventory, Action<int, Item> action)
    {
        _inventoryDictionary[inventory].OnInventoryChanged += action;
    }
    public void RemoveChangeListener(EInventory inventory, Action<int, Item> action)
    {
        _inventoryDictionary[inventory].OnInventoryChanged -= action;
    }
}

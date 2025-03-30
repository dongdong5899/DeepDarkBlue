using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public EInventory inventoryEnum;
    [SerializeField] private Transform _inventoryTrm;
    [SerializeField] private Vector2Int _inventorySize;

    [field: SerializeField] public EItemType ItemType { get; private set; }

    private List<Slot> _slots;
    private Dictionary<EItemID, int> _itemCountDictionary;

    public Transform dragItemTrm;
    public Action<int, Item> OnInventoryChanged;

    public void Init()
    {
        _slots = new List<Slot>();
        _itemCountDictionary = new Dictionary<EItemID, int>();
        for (int i = 0; i < _inventorySize.y * _inventorySize.x; i++)
        {
            Slot slot = _inventoryTrm.GetChild(i).GetComponent<Slot>();
            slot.SlotInit(this);
            slot.OnSlotChangedEvent += prevItem => OnInventoryChanged?.Invoke(i, prevItem);
            _slots.Add(slot);
        }
    }

    public Item AddItem(Item item)
    {
        if (((int)ItemType & (int)item.itemSO.itemType) == 0) return item;

        int startAmount = item.Amount;

        for (int i = 0; i < _inventorySize.y * _inventorySize.x; i++)
        {
            if (item.Amount == 0)
            {
                AddItemAmount(item.itemSO.itemID, startAmount);
                return null;
            }
            if (_slots[i].TryGetAssignedItem(out Item slotItem) &&
                slotItem.IsSameItem(item) && slotItem.IsFull() == false)
            {
                int remain = slotItem.AddAmount(item.Amount);
                item.SetAmount(remain);
            }
        }

        if (item.Amount == 0)
        {
            AddItemAmount(item.itemSO.itemID, startAmount);
            return null;
        }

        for (int i = 0; i < _inventorySize.y * _inventorySize.x; i++)
        {
            if (_slots[i].GetAssignedItem() == null)
            {
                _slots[i].AssignItem(item);
                AddItemAmount(item.itemSO.itemID, startAmount);
                return null;
            }
        }

        AddItemAmount(item.itemSO.itemID, startAmount - item.Amount);
        return item;
    }

    public Item RemoveItem(Item item)
    {
        int startAmount = item.Amount;
        for (int i = _inventorySize.y * _inventorySize.x - 1; i >= 0; i--)
        {
            if (item.Amount == 0)
            {
                SubtractItemAmount(item.itemSO.itemID, startAmount);
                return null;
            }
            if (_slots[i].TryGetAssignedItem(out Item slotItem) &&
                slotItem.IsSameItem(item) && slotItem.Amount > 0)
            {
                int remain = slotItem.RemoveAmount(item.Amount);
                if (slotItem.Amount == 0)
                    _slots[i].AssignItem(null);
                item.SetAmount(remain);
            }
        }
        SubtractItemAmount(item.itemSO.itemID, startAmount - item.Amount);

        return item;
    }

    public void AddItemAmount(EItemID eItemName, int amount)
    {
        if (!_itemCountDictionary.TryAdd(eItemName, amount))
            _itemCountDictionary[eItemName] += amount;
    }
    public void SubtractItemAmount(EItemID eItemName, int amount)
    {
        if (_itemCountDictionary.ContainsKey(eItemName))
        {
            _itemCountDictionary[eItemName] -= amount;
            if (_itemCountDictionary[eItemName] < 0) _itemCountDictionary[eItemName] = 0;
        }
    }



    public int GetItemAmount(EItemID eItemName)
    {
        if (_itemCountDictionary.TryGetValue(eItemName, out int itemAmount))
            return itemAmount;
        else
            return 0;
    }

    public Slot GetSlot(int index)
    {
        if (_slots.Count > index)
        {
            return _slots[index];
        }
        return null;
    }
    public Slot GetSlot(Vector2Int pos)
    {
        if (_slots.Count / _inventorySize.x > pos.y &&
            _slots.Count / _inventorySize.y > pos.x)
        {
            return _slots[pos.x + pos.y * _inventorySize.x];
        }
        return null;
    }
}
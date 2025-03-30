using System;

public class Item
{
    public ItemSO itemSO;
    public Action<int, int> OnChangedAmount;
    public Action<int> OnChangedDurability;
    public int Amount { get; private set; }
    public int CurrentDurability { get; private set; }

    public void ItemInit(ItemSO startItemSO, int amount = 1)
    {
        itemSO = startItemSO;
        Amount = amount;
        CurrentDurability = itemSO.durability;
    }

    public void Use()
    {
        CurrentDurability--;
        if (CurrentDurability <= 0)
        {
            CurrentDurability = itemSO.durability;
            RemoveAmount(1);
        }
        OnChangedDurability?.Invoke(CurrentDurability);
    }

    public int AddAmount(int amount)
    {
        int prevAmount = Amount;
        int remain = 0;
        Amount += amount;
        if (Amount > itemSO.maxOverlapAmount)
        {
            remain = Amount - itemSO.maxOverlapAmount;
            Amount = itemSO.maxOverlapAmount;
        }
        OnChangedAmount?.Invoke(prevAmount, Amount);
        return remain;
    }
    public int RemoveAmount(int amount)
    {
        int prevAmount = Amount;
        int over = 0;
        Amount -= amount;
        if (Amount < 0)
        {
            over = -Amount;
            Amount = 0;
        }
        OnChangedAmount?.Invoke(prevAmount, Amount);
        return over;
    }
    public void SetAmount(int amount)
    {
        int prevAmount = Amount;
        Amount = amount;
        if (Amount > itemSO.maxOverlapAmount)
        {
            Amount = itemSO.maxOverlapAmount;
        }
        OnChangedAmount?.Invoke(prevAmount, Amount);
    }
    public void SetDurability(int durability)
    {
        CurrentDurability = durability;
        OnChangedDurability?.Invoke(CurrentDurability);
    }
    public bool IsSameItem(Item targetItem)
        => itemSO.itemID == targetItem.itemSO.itemID;
    public bool IsFull()
        => Amount == itemSO.maxOverlapAmount;

    public Item Clone()
    {
        Item item = new Item();
        item.itemSO = itemSO;
        item.Amount = Amount;
        item.CurrentDurability = CurrentDurability;
        return item;
    }
}

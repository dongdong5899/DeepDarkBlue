using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform _itemRectTrm;
    [SerializeField] private Image _selectOutline;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private TextMeshProUGUI _durabilityText;

    private Image _itemImage;
    public Inventory Inventory { get; private set; }
    private Item _assignedItem;
    private Vector3 _dragStartPos;
    private bool _isDrag;

    public event Action<Item> OnSlotChangedEvent;

    public void SlotInit(Inventory inventory)
    {
        Inventory = inventory;
        _selectOutline.color = new Color(1, 1, 1, 0);
        _itemImage = _itemRectTrm.GetComponent<Image>();
        AssignItem(null);
    }

    public void AssignItem(Item item)
    {
        if (_assignedItem != null)
        {
            _assignedItem.OnChangedAmount -= HandleChangedAmountEvent;
            _assignedItem.OnChangedDurability -= SetDurabilityText;
        }

        if (item == null)
        {
            _itemImage.color = new Color(1, 1, 1, 0);
            _amountText.text = "";
            _durabilityText.text = "";
        }
        else
        {
            _itemImage.sprite = item.itemSO.itemSprite;
            _itemImage.color = new Color(1, 1, 1, 1);
            _amountText.text = item.Amount == 1 ? "" : item.Amount.ToString();
            if (item.itemSO.itemType == EItemType.Useable)
            {
                SetDurabilityText(item.CurrentDurability);
            }
        }
        _assignedItem = item;
        OnSlotChangedEvent?.Invoke(_assignedItem);

        if (_assignedItem != null)
        {
            _assignedItem.OnChangedAmount += HandleChangedAmountEvent;
            _assignedItem.OnChangedDurability += SetDurabilityText;
        }
    }

    public void SetDurabilityText(int durability)
    {
        _durabilityText.text = durability.ToString();
    }

    private void HandleChangedAmountEvent(int prevAmount, int currentAmount)
    {
        if (currentAmount == 0)
            AssignItem(null);
        else
            _amountText.text = currentAmount == 1 ? "" : currentAmount.ToString();
        OnSlotChangedEvent?.Invoke(_assignedItem);
    }

    public void ChangeSlot(Slot slot)
    {
        Item item = slot.GetAssignedItem()?.Clone();

        if ((item != null && ((int)Inventory.ItemType & (int)item.itemSO.itemType) == 0) ||
            (_assignedItem != null && ((int)slot.Inventory.ItemType & (int)_assignedItem.itemSO.itemType) == 0)) 
            return;

            Inventory.AddItemAmount(item.itemSO.itemID, item.Amount);
        slot.Inventory.SubtractItemAmount(item.itemSO.itemID, item.Amount);
        if (_assignedItem != null)
        {
            Inventory.SubtractItemAmount(_assignedItem.itemSO.itemID, _assignedItem.Amount);
            slot.Inventory.AddItemAmount(_assignedItem.itemSO.itemID, _assignedItem.Amount);
        }

        slot.AssignItem(_assignedItem);
        AssignItem(item);
    }

    public bool TryGetAssignedItem(out Item item)
    {
        item = _assignedItem;
        return _assignedItem != null;
    }
    public Item GetAssignedItem()
    {
        return _assignedItem;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isDrag) return;
        _selectOutline.color = new Color(1, 1, 0, 0.8f);
        InventoryManager.Instance.PointerSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isDrag) return;
        _selectOutline.color = new Color(1, 1, 0, 0f);
        InventoryManager.Instance.PointerSlot = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_assignedItem == null) return;

        _isDrag = true;
        _selectOutline.color = new Color(1, 1, 0, 0f);
        _dragStartPos = Input.mousePosition;
        InventoryManager.Instance.DragSlot = this;
        _itemRectTrm.SetParent(Inventory.dragItemTrm);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isDrag)
        {
            InventoryManager.Instance.DragSlot = null;
            Slot pointerSlot = InventoryManager.Instance.PointerSlot;
            if (pointerSlot != null && pointerSlot != this)
            {
                Item item = pointerSlot.GetAssignedItem();
                if (item != null && item.IsSameItem(_assignedItem))
                {
                    int prevAmount = _assignedItem.Amount;
                    int remain = item.AddAmount(_assignedItem.Amount);
                    if (remain == 0)
                        AssignItem(null);
                    else
                        _assignedItem.SetAmount(remain);

                    int moveAmount = prevAmount - remain;
                    Inventory.SubtractItemAmount(item.itemSO.itemID, moveAmount);
                    pointerSlot.Inventory.AddItemAmount(item.itemSO.itemID, moveAmount);
                }
                else
                {
                    pointerSlot.ChangeSlot(this);
                }
            }
            _itemRectTrm.SetParent(transform);
        }

        _itemRectTrm.anchoredPosition = Vector3.zero;
        _isDrag = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDrag == false) return;
        Vector2 pos = Input.mousePosition;
        float screenRatio = (float)1920 / Screen.width;
        pos.x *= screenRatio;
        pos.y *= screenRatio;
        _itemRectTrm.anchoredPosition = pos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_assignedItem == null) return;

        if (Inventory.inventoryEnum == EInventory.Main)
        {
            Item item;
            for (int i = (int)EInventory.Quick; i < (int)EInventory.FootArmor + 1; i++)
            {
                item = InventoryManager.Instance.AddItem((EInventory)i, _assignedItem);

                if (item == null)
                {
                    AssignItem(null);
                    break;
                }
            }
        }
        else if ((int)Inventory.inventoryEnum >= (int)EInventory.Quick)
        {
            Item item = InventoryManager.Instance.AddItem(EInventory.Main, _assignedItem);
            if (item == null) AssignItem(null);
        }
    }
}

using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name, _count;
    [SerializeField] private Image _image;
    private int _needCount;
    private ItemSO _itemSO;

    public void Init(ItemSO itemSO, int count)
    {
        _image.sprite = itemSO.itemSprite;
        _name.text = itemSO.itemName;
        _itemSO = itemSO;
        _needCount = count;
    }

    private void Update()
    {
        CountTextUpdate(InventoryManager.Instance.GetItemAmount(EInventory.Main, _itemSO.itemID));
    }

    private void CountTextUpdate(int invenCount)
    {
        _count.text = $"{invenCount}/{_needCount.ToString()}";
        _count.color = _needCount > invenCount ? Color.red : Color.white;
    }
}

using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : ActiveTogleUI
{
    [SerializeField] private ItemListSO _itemListSO;
    [SerializeField] private RecipeUI _recipeUI;
    [SerializeField] private NeedResourceUI _needResourceUI;
    [SerializeField] private Transform _recipeTrm;
    [SerializeField] private Transform _needResourceTrm;
    [SerializeField] private Button _craftBtn;

    private ItemSO _currentTargetItem;

    public override void Close()
    {
        base.Close();
        SettingRecipe(null);
    }

    private void Awake()
    {
        _craftBtn.onClick.AddListener(CraftItem);

        int recipeCount = 0;
        foreach (ItemSO itemSO in _itemListSO.ItemList)
        {
            if (itemSO.isCraftable)
            {
                recipeCount++;
                RecipeUI recipeUI = Instantiate(_recipeUI, _recipeTrm);
                recipeUI.Init(this, itemSO);
            }
        }
        RectTransform rectTrm = _recipeTrm as RectTransform;
        rectTrm.sizeDelta = new Vector2(rectTrm.sizeDelta.x, 85 * recipeCount + 45);
    }

    public void SettingRecipe(ItemSO itemSO)
    {
        _currentTargetItem = itemSO;
        for (int i = 0; i < _needResourceTrm.childCount; i++)
        {
            Destroy(_needResourceTrm.GetChild(i).gameObject);
        }
        if (itemSO != null)
        {
            foreach (ItemSO recipeItemSO in itemSO.recipeDict.Keys)
            {
                NeedResourceUI needResourceUI = Instantiate(_needResourceUI, _needResourceTrm);
                needResourceUI.Init(recipeItemSO, itemSO.recipeDict[recipeItemSO]);
            }
        }
    }

    private void CraftItem()
    {
        if (_currentTargetItem == null) return;

        bool canCraft = true;
        foreach (ItemSO recipeItemSO in _currentTargetItem.recipeDict.Keys)
        {
            int inventoryCount = InventoryManager.Instance.GetItemAmount(EInventory.Main, recipeItemSO.itemID);
            if (inventoryCount < _currentTargetItem.recipeDict[recipeItemSO])
            {
                canCraft = false;
                break;
            }
        }

        if (canCraft)
        {
            foreach (ItemSO recipeItemSO in _currentTargetItem.recipeDict.Keys)
            {
                int itemCount = _currentTargetItem.recipeDict[recipeItemSO];
                InventoryManager.Instance.RemoveItem(EInventory.Main, recipeItemSO, itemCount);
            }

            InventoryManager.Instance.AddItem(EInventory.Main, _currentTargetItem);
        }
    }
}

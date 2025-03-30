using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _outline;
    private CraftingUI _craftingUI;
    private Button _recipeBtn;
    private ItemSO _itemSO;

    public void Init(CraftingUI craftingUI, ItemSO itemSO)
    {
        _itemSO = itemSO;
        _name.text = itemSO.itemName;
        _icon.sprite = itemSO.itemSprite;
        _craftingUI = craftingUI;
        _recipeBtn = GetComponent<Button>();
        _recipeBtn.onClick.AddListener(() => _craftingUI.SettingRecipe(_itemSO));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _outline.color = Color.yellow;
    }

    private void OnDisable()
    {
        _outline.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _outline.color = Color.white;
    }
}

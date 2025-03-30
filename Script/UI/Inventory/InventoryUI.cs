using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MoveTogleUI
{
    [Header("InventoryUI setting")]
    [SerializeField] private Button _changeBtn;
    private TextMeshProUGUI _changeBtnText;

    private void Awake()
    {
        _changeBtnText = _changeBtn.transform.GetComponentInChildren<TextMeshProUGUI>();
        _changeBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.Togle(ETogleUIName.Equip);
            bool isCraftingOpen = UIManager.Instance.Togle(ETogleUIName.Crafting);
            _changeBtnText.text = isCraftingOpen ? "장비" : "제작";
        });
    }
}

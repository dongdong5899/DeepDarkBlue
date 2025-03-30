using TMPro;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{
    // 키 도움말 "열기[E]"
    [SerializeField] private GameObject interactionObj;
    [SerializeField] private ItemSO _itemSO;
    [SerializeField] private int _count;
    [SerializeField] private SpriteRenderer _visual;

    private bool _isInitialized;

    public Transform Transform { get => transform; set { } }

    private void Awake()
    {
        if (_isInitialized == false)
            Init(_itemSO, _count);

        if (_count > 20)
            _count = 1;
    }

    public void Interaction()
    {
        if (_itemSO.itemType == EItemType.Consumable)
        {
            _itemSO.onConsume?.Invoke();
        }
        else
        {
            InventoryManager.Instance.AddItem(EInventory.Main, _itemSO, _count);
        }
        Destroy(gameObject);
    }

    public void SetInteractable(bool isInteractable)
    {
        interactionObj.SetActive(isInteractable);
    }

    public void Init(ItemSO itemSo, int count)
    {
        _isInitialized = true;
        _itemSO = itemSo;
        _visual.sprite = itemSo.itemSprite;
        _count = count;
    }

    public void HealHp(int val)
    {
        GameManager.Instance.Player.curHp += val;
        if (GameManager.Instance.Player.curHp > GameManager.Instance.Player.maxHp)
        {
            GameManager.Instance.Player.curHp = GameManager.Instance.Player.maxHp;
        }
        UIManager.Instance.SetHealthAmount(GameManager.Instance.Player.curHp);
    }
}

using DG.Tweening;
using UnityEngine;

public enum ETogleUIName
{
    Inventory,
    Equip,
    Crafting,
    Option,
    ESC
}

public abstract class TogleUI : MonoBehaviour
{
    public RectTransform RectTransform { get; private set; }

    [field:SerializeField] public ETogleUIName TogleUIName { get; private set; }

    [SerializeField] private bool _isOpenStart;
    public bool IsOpened { get; private set; }

    protected virtual void Start()
    {
        RectTransform = transform as RectTransform;

        IsOpened = _isOpenStart;
        Init(_isOpenStart);
    }

    public abstract void Init(bool isOpened);

    public bool Togle()
    {
        bool isOpen;
        if (IsOpened)
        {
            isOpen = false;
            Close();
        }
        else
        {
            isOpen = true;
            Open();
        }

        return isOpen;
    }

    public virtual void Open()
    {
        IsOpened = true;
    }
    public virtual void Close()
    {
        IsOpened = false;
    }
}

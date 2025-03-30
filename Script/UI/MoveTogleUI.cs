using DG.Tweening;
using UnityEngine;

public class MoveTogleUI : TogleUI
{
    [SerializeField] protected Vector2 _openPos;
    [SerializeField] protected Vector2 _closePos;

    public override void Init(bool isOpenStart)
    {
        RectTransform.anchoredPosition = isOpenStart ? _openPos : _closePos;
    }

    public override void Open()
    {
        base.Open();
        RectTransform.DOKill();
        RectTransform.DOAnchorPos(_openPos, 0.2f).SetEase(Ease.OutCirc);
    }

    public override void Close()
    {
        base.Close();
        RectTransform.DOKill();
        RectTransform.DOAnchorPos(_closePos, 0.2f).SetEase(Ease.InCirc);
    }
}

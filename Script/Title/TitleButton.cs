using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _button;
    private CanvasGroup _group;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _group = GetComponent<CanvasGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _group.alpha = 1;
        TitleManager.Instance.SelectButton(_button);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _group.alpha = 0.05f;
        TitleManager.Instance.SelectButton(null);
    }
}

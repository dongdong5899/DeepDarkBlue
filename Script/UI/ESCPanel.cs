using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESCPanel : ActiveTogleUI
{
    [SerializeField] private Button _gameStartBtn, _optionBtn, _exitBtn;

    public override void Close()
    {
        base.Close();
        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }

    public override void Open()
    {
        base.Open();
        Time.timeScale = 0;
    }

    private void Awake()
    {
        _gameStartBtn.onClick.AddListener(() => UIManager.Instance.CloseUI(ETogleUIName.ESC));
        _optionBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseUI(ETogleUIName.ESC);
            UIManager.Instance.OpenUI(ETogleUIName.Option);
        });
        _exitBtn.onClick.AddListener(() => SceneManager.LoadScene("Title"));
    }


}

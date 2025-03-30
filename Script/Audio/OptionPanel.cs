using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : ActiveTogleUI
{
    [SerializeField] private Slider[] _sliders;
    [SerializeField] private TextMeshProUGUI[] _valueTexts;
    [SerializeField] private Toggle _flasgToggle;
    [SerializeField] private Button _exitBtn;

    protected override void Start()
    {
        base.Start();

        VolumeSaveData volumeSaveData = AudioManager.Instance.VolumeSaveData;
        if (GameManager.Instance != null)
        {
            TogleSaveData toggleSaveData = GameManager.Instance.ToggleSaveData;

            _flasgToggle.isOn = toggleSaveData.isFlashToggle;
            _flasgToggle.onValueChanged.AddListener(HandleFlashToggleChangedEvent);
        }

        for (int i = 0; i < _sliders.Length; i++)
        {
            int index = i;
            EAudioType eAudioType = (EAudioType)index;
            float value = volumeSaveData[eAudioType];
            _sliders[index].value = value;
            _valueTexts[index].text = Mathf.CeilToInt(value * 100).ToString();

            _sliders[index].onValueChanged.AddListener
                (value =>
                {
                    AudioManager.Instance.SetVolume(eAudioType, value);
                    _valueTexts[index].text = Mathf.CeilToInt(value * 100).ToString();
                });
        }

        _exitBtn.onClick.AddListener(() =>
        {
            if (UITogleManager.Instance == null)
            {
                UIManager.Instance.CloseUI(ETogleUIName.Option);
                UIManager.Instance.OpenUI(ETogleUIName.ESC);
            }
            else
            {
                UITogleManager.Instance.CloseUI(ETogleUIName.Option);
            }
        });
    }

    private void HandleFlashToggleChangedEvent(bool isOn)
    {
        GameManager.Instance.SetToggle(isOn);
    }

    public override void Open()
    {
        base.Open();
        Time.timeScale = 0;
    }

    public override void Close()
    {
        base.Close();
        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 하드코딩된 클래스
public class TitleManager : MonoSingleton<TitleManager>
{
    [SerializeField] private Button btn_start;
    [SerializeField] private Button btn_setting;
    [SerializeField] private Button btn_exit;

    [SerializeField] private Light2D light2D;

    private int selectNumber = 0;

    private bool isStarted = false;

    private Tween _lightTween;

    private void Awake()
    {
        btn_start.onClick.AddListener(OnClickStartButton);
        btn_setting.onClick.AddListener(OnClickSettingButton);
        btn_exit.onClick.AddListener(OnClickExitButton);

        light2D.intensity = 0;
    }

    private void Start()
    {
        AudioManager.Instance.PlaySound(EAudioName.TitleBGM, transform, transform.position);
    }

    public void SelectButton(Button button)
    {
        if (_lightTween != null && _lightTween.IsActive()) _lightTween.Kill();
        if (button == null)
        {
            _lightTween = DOTween.To(() => light2D.intensity, value => light2D.intensity = value, 0, 1f);
        }
        else
        {
            light2D.transform.up = button.transform.position - light2D.transform.position;
            light2D.intensity = 0.5f;
        }
    }

    private void OnClickStartButton()
    {
        isStarted = true;
        AudioManager.Instance.PlaySound(EAudioName.Click, transform, transform.position);
        SceneManager.LoadScene("Main");
    }
    private void OnClickSettingButton()
    {
        AudioManager.Instance.PlaySound(EAudioName.Click, transform, transform.position);
        UITogleManager.Instance.OpenUI(ETogleUIName.Option);
    }
    private void OnClickExitButton()
    {
        AudioManager.Instance.PlaySound(EAudioName.Click, transform, transform.position);
        Application.Quit();
    }
}

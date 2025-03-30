using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private InputReaderSO _inputReader;
    [SerializeField] private Image img_flashLightGauge;
    [SerializeField] private Image img_hpGauge;
    [SerializeField] private Image img_staminaGauge;

    public TMP_Text txt_km;

    [SerializeField] private DamageText _damageText;

    // 인벤토리
    private Dictionary<ETogleUIName, TogleUI> _togleUIDictionary;

    private void Awake()
    {
        _togleUIDictionary = new Dictionary<ETogleUIName, TogleUI>();

        TogleUI[] togleUIs = FindObjectsByType<TogleUI>(FindObjectsSortMode.None);
        foreach (TogleUI togleUI in togleUIs)
        {
            _togleUIDictionary.Add(togleUI.TogleUIName, togleUI);
        }
        _inputReader.OnESCEvent += HandleESCEvent;
    }

    public void SetInput()
    {
        _inputReader.OnInventoryEvent += HandleInventoryEvent;
    }

    private void OnDestroy()
    {
        _inputReader.OnInventoryEvent -= HandleInventoryEvent;
        _inputReader.OnESCEvent -= HandleESCEvent;
    }

    private void HandleESCEvent()
    {
        if (IsOpenedUI(ETogleUIName.Inventory))
            CloseUI(ETogleUIName.Inventory);
        else if (IsOpenedUI(ETogleUIName.Option))
            CloseUI(ETogleUIName.Option);
        else 
            Togle(ETogleUIName.ESC);
    }

    private void HandleInventoryEvent()
    {
        if (IsOpenedUI(ETogleUIName.ESC) || IsOpenedUI(ETogleUIName.Option)) return;
        Togle(ETogleUIName.Inventory);
    }

    public void Update()
    {
        DepthTextUpdate();
    }

    public void CreateDamageText(int damage, float size, Vector3 pos)
    {
        DamageText damageText = Instantiate(_damageText, pos, Quaternion.identity);
        damageText.Init(damage, size);
    }

    /// <summary>
    /// 체력과 손전등, 수심 계산
    /// </summary>
    private void DepthTextUpdate()
    {
        txt_km.text = (-(int)(GameManager.Instance.PlayerTrm.position.y - 50) * 100).ToString() + " m";
    }

    public void SetHealthAmount(float amount)
    {
        img_hpGauge.fillAmount = amount;
    }

    public void SetStaminaAmount(float amount)
    {
        img_staminaGauge.fillAmount = amount;
    }

    public void SetLightBatteryAmount(float amount)
    {
        img_flashLightGauge.fillAmount = amount;
    }

    public void Die()
    {
        Fade.Out(1f);
    }

    #region inventory toggle

    public bool IsOpenedUI(ETogleUIName eTogleUIName)
    {
        return _togleUIDictionary[eTogleUIName].IsOpened;
    }
    public bool Togle(ETogleUIName eTogleUIName)
    {
        return _togleUIDictionary[eTogleUIName].Togle();
    }
    public void OpenUI(ETogleUIName eTogleUIName)
    {
        _togleUIDictionary[eTogleUIName].Open();
    }
    public void CloseUI(ETogleUIName eTogleUIName)
    {
        _togleUIDictionary[eTogleUIName].Close();
    }
    #endregion
}

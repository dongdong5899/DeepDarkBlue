using System.Collections.Generic;
using UnityEngine;

public class UITogleManager : MonoSingleton<UITogleManager>
{
    private Dictionary<ETogleUIName, TogleUI> _togleUIDictionary;

    private void Awake()
    {
        _togleUIDictionary = new Dictionary<ETogleUIName, TogleUI>();
        TogleUI[] togleUIs = GameObject.FindObjectsByType<TogleUI>(FindObjectsSortMode.None);
        foreach (TogleUI togleUI in togleUIs)
        {
            _togleUIDictionary.Add(togleUI.TogleUIName, togleUI);
        }
    }

    public bool IsOpenedUI(ETogleUIName eTogleUIName)
    {
        return _togleUIDictionary[eTogleUIName].IsOpened;
    }
    public void Togle(ETogleUIName eTogleUIName)
    {
        _togleUIDictionary[eTogleUIName].Togle();
    }
    public void OpenUI(ETogleUIName eTogleUIName)
    {
        _togleUIDictionary[eTogleUIName].Open();
    }
    public void CloseUI(ETogleUIName eTogleUIName)
    {
        _togleUIDictionary[eTogleUIName].Close();
    }
}

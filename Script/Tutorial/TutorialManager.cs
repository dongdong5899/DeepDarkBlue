using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [field: SerializeField] public InputReaderSO InputReader { get; private set; }

    [SerializeField] private TMP_Text txt_hint;

    int level = 0;

    int count = 0;

    void Start()
    {

        GameManager.Instance.Player.InputReader.OnQuickSlotEvent += HandleQuickSlotEvent;
        GameManager.Instance.Player.InputReader.OnFireEvent += HandleFireEvent;
        GameManager.Instance.Player.InputReader.OnFlashEvent += HandleFlashEvent;
        GameManager.Instance.Player.InputReader.OnSprintEvent += HandleSprintEvent;
        GameManager.Instance.Player.InputReader.OnInventoryEvent += HandleInventoryEvent;
        GameManager.Instance.Player.InputReader.OnInteractionEvent += HandleInteractionEvent;

        StartCoroutine(ShowTextOnce("[WASD]로 움직여 심해 더 깊은 곳으로 들어가 한계에 도전하세요"));
    }

    private void OnDestroy()
    {
        GameManager.Instance.Player.InputReader.OnQuickSlotEvent -= HandleQuickSlotEvent;
        GameManager.Instance.Player.InputReader.OnFireEvent -= HandleFireEvent;
        GameManager.Instance.Player.InputReader.OnFlashEvent -= HandleFlashEvent;
        GameManager.Instance.Player.InputReader.OnSprintEvent -= HandleSprintEvent;
        GameManager.Instance.Player.InputReader.OnInventoryEvent -= HandleInventoryEvent;
        GameManager.Instance.Player.InputReader.OnInteractionEvent -= HandleInteractionEvent;
    }

    private void Update()
    {
        if (GameManager.Instance.PlayerTrm.position.y < -10 && level == 0)
        {
            level++;
            StartCoroutine(ShowTextOnce("[마우스 우클릭]으로 손전등을 꺼내어 어둠을 밝혀보세요"));
        }


    }
    private void HandleFlashEvent(bool isOn)
    {
        if (level != 1) return;

        StartCoroutine(ShowTextOnce("[F]로 잡동사니를 주워 강해지세요"));

        level++;
    }
    private void HandleInteractionEvent()
    {
        if (level != 2) return;
        StartCoroutine(ShowTextOnce("[E]를 눌러 제작 버튼을 누르고 아이템을 만드세요"));
        level++;
    }
    private void HandleInventoryEvent()
    {
        if (level != 3) return;
        StartCoroutine(ShowTextOnce("조합한 아이템을 퀵 슬롯에 넣어 사용하세요"));
        level++;
    }
    
    private void HandleQuickSlotEvent(int index)
    {
        if (level != 4) return;
        StartCoroutine(ShowTextOnce("[Shif]를 누른 채로 대시를 써서 더 빠르게 이동하세요"));
        level++;
    }

    private void HandleSprintEvent(bool isOn)
    {
        if (level != 5) return;

        Debug.Log("끝");
    }



    private void HandleFireEvent()
    {
        
    }


    private IEnumerator ShowTextOnce(string str)
    {
        txt_hint.text = str; // "심해 깊은 곳의 비밀을 당신의 손으로 비춰보세요.";

        yield return new WaitForSeconds(1);
        //txt_hint.text = "심해 깊은 곳으로 이동하여 새로운 지역을 발견하세요.";
    }
}

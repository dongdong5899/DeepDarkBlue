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

        StartCoroutine(ShowTextOnce("[WASD]�� ������ ���� �� ���� ������ �� �Ѱ迡 �����ϼ���"));
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
            StartCoroutine(ShowTextOnce("[���콺 ��Ŭ��]���� �������� ������ ����� ����������"));
        }


    }
    private void HandleFlashEvent(bool isOn)
    {
        if (level != 1) return;

        StartCoroutine(ShowTextOnce("[F]�� �⵿��ϸ� �ֿ� ����������"));

        level++;
    }
    private void HandleInteractionEvent()
    {
        if (level != 2) return;
        StartCoroutine(ShowTextOnce("[E]�� ���� ���� ��ư�� ������ �������� ���弼��"));
        level++;
    }
    private void HandleInventoryEvent()
    {
        if (level != 3) return;
        StartCoroutine(ShowTextOnce("������ �������� �� ���Կ� �־� ����ϼ���"));
        level++;
    }
    
    private void HandleQuickSlotEvent(int index)
    {
        if (level != 4) return;
        StartCoroutine(ShowTextOnce("[Shif]�� ���� ä�� ��ø� �Ἥ �� ������ �̵��ϼ���"));
        level++;
    }

    private void HandleSprintEvent(bool isOn)
    {
        if (level != 5) return;

        Debug.Log("��");
    }



    private void HandleFireEvent()
    {
        
    }


    private IEnumerator ShowTextOnce(string str)
    {
        txt_hint.text = str; // "���� ���� ���� ����� ����� ������ ���纸����.";

        yield return new WaitForSeconds(1);
        //txt_hint.text = "���� ���� ������ �̵��Ͽ� ���ο� ������ �߰��ϼ���.";
    }
}

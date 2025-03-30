using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public enum Status
{
    Idle,
    Atk1,
    Atk2,
    Atk3,
    None
}
public class TheLightEaterBeHave : MonoBehaviour
{
    public int challengeTime;
    private Status acting;
    [SerializeField] private Animator _ani;
    [SerializeField] private Image gauge;
    [SerializeField] private TextMeshProUGUI _timeTxt;

    private void Start()
    {
        AudioManager.Instance.PlaySound(EAudioName.BossBGM, gameObject.transform);
        _timeTxt.text = challengeTime + "초";
        gauge.fillAmount = 1;
        StartCoroutine(ChallengeFlow());
    }

    private IEnumerator ChallengeFlow()
    {
        var tempTime = challengeTime;
        while (tempTime > 0)
        {
            yield return new WaitForSeconds(1f);
            tempTime--;
            _timeTxt.text = tempTime + "초";
            gauge.fillAmount = 1 - (float)tempTime / challengeTime;
        }

        if (!Objtacle.isDead)
        {
            SceneManager.LoadScene(5);
        }
        
    }
    private void Update()
    {
    }

    public void Think()
    {
        if (acting==Status.None)
        {
            var i = Random.Range(0, 4);
            switch (i)
            {
                case 0:
                    _ani.SetInteger("behave", 0);
                    acting = Status.Idle;
                    break;
                case 1:
                    _ani.SetInteger("behave", 1);
                    _ani.SetTrigger("Attack1");
                    acting = Status.Atk1;
                    break;
                case 2:
                    _ani.SetInteger("behave", 2);
                    _ani.SetTrigger("Attack2");
                    acting = Status.Atk2;
                    break;
                case 3:
                    _ani.SetInteger("behave", 3);
                    _ani.SetTrigger("Attack3");
                    acting = Status.Atk3;
                    break;
            }

        }
    }

    public void EndPattern()
    {
        acting = Status.None;
        _ani.SetInteger("behave", 0);
        Think();
    }
}

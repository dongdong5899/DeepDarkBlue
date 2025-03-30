using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    private Transform playerTrm;

    private bool activateFlag = false;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerTrm = GameManager.Instance.PlayerTrm;
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position,playerTrm.position) < 7 && !activateFlag)
        {
            activateFlag = true;
            animator.SetTrigger("Appear");

            this.enabled = false;
        }
    }
}

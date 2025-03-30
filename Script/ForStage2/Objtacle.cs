using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objtacle : MonoBehaviour
{
    public static bool isDead;

    public void Start()
    {
        isDead = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")&&!isDead)
        {
            isDead = true;
            StartCoroutine(DeathFlow());
        }
    }

    private IEnumerator DeathFlow()
    {
        Fade.In(10f);
        yield return new WaitForSeconds(0.1f);
        isDead = false;
        Fade.Out(10f);
        SceneManager.LoadScene(3);
    }
}

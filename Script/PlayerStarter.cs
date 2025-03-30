using System;
using UnityEngine;

public class PlayerStarter : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.Player.SetInput();
        UIManager.Instance.SetInput();
    }
}

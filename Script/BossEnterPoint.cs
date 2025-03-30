using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnterPoint : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _bossEnterPoint;
    public Transform Transform { get => transform; set { } }

    public void Interaction()
    {
        SceneManager.LoadScene("TheRaceCutScene");
    }

    public void SetInteractable(bool isInteractable)
    {
        _bossEnterPoint.SetActive(isInteractable);
    }
}

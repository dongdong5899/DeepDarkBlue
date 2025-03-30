using System.Collections;
using UnityEngine;

public class StartTutorial : MonoBehaviour
{
    [SerializeField] private GameObject _inGameUI;
    [SerializeField] private float _downSpeed;
    [SerializeField] private float _pullDownY;
    private bool _isEnd = false;

    private void Start()
    {
        _inGameUI.SetActive(false);
    }

    private void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * _downSpeed;

        if (transform.position.y < _pullDownY && _isEnd == false)
        {
            _isEnd = true;
            CameraManager.Instance.ChangeCamera("Player");

            StartCoroutine(ControlStartCoroutine(3f));
        }
    }

    private IEnumerator ControlStartCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.Player.SetInput();
        UIManager.Instance.SetInput();
        _inGameUI.SetActive(true);
    }
}

using System.Collections;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] private Transform _textTrigger;
    [SerializeField] private GameObject _blackBoard;
    [SerializeField] private ParticleSystem _breakParticle;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _pullDownY;
    private bool _isEnd = false;
    private bool _isStart = false;

    private float _currentTime;

    private void Update()
    {
        if (_isEnd) return;

        if (_isStart == false)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > 1 && _textTrigger.position.y > 3)
            {
                _textTrigger.position = new Vector3(0, 2);
            }
            if (_currentTime > 7 && _textTrigger.position.y > 1)
            {
                _textTrigger.position = new Vector3(0, 1);
            }

            if (_currentTime > 13)
            {
                _isStart = true;
                _blackBoard.SetActive(false);
                _breakParticle.Play();
            }
        }
        else
        {
            transform.position += Vector3.up * Time.deltaTime * _moveSpeed;

            if (transform.position.y < _pullDownY && _isEnd == false)
            {
                _isEnd = true;

                StartCoroutine(EndCoroutine(5f));
            }
        }
    }

    private IEnumerator EndCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }
}

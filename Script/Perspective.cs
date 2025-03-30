using UnityEngine;

public class Perspective : MonoBehaviour
{
    [SerializeField] private float _ratio;
    [SerializeField] private Transform _target;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        transform.localScale = Vector3.one * (1 - _ratio);
    }

    private void FixedUpdate()
    {
        transform.position = _startPosition + (_target.position - _startPosition) * _ratio;
    }
}

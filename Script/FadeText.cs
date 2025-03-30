using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadeText : MonoBehaviour
{
    [SerializeField] private Transform _targetTrm;
    [SerializeField] private float _radius;
    [SerializeField] private float _popUpFade;
    [SerializeField] private float _popUpDuration;
    private float _popUpTime;
    private List<TextMeshPro> _textMeshProList;
    private List<SpriteRenderer> _spriteRendererList;

    private bool _isVisible;
    private bool _isEnd;

    private void Awake()
    {
        _textMeshProList = new List<TextMeshPro>();
        _spriteRendererList = new List<SpriteRenderer>();
        GetComponentsInChildren(_textMeshProList);
        GetComponentsInChildren(_spriteRendererList);
        _textMeshProList.ForEach(text => text.alpha = 0.0f);
        _spriteRendererList.ForEach(renderer =>
        {
            Color color = renderer.color;
            color.a = 0;
            renderer.color = color;
        });
    }

    private void Update()
    {
        if (_isEnd) return;

        if (_isVisible == false && Vector2.Distance(_targetTrm.position, transform.position) < _radius)
        {
            _isVisible = true;
            _textMeshProList.ForEach(text => text.DOFade(1, _popUpFade));
            _spriteRendererList.ForEach(renderer => renderer.DOFade(1, _popUpFade));
            _popUpTime = Time.time;
        }

        if (_isVisible && _popUpTime + _popUpDuration < Time.time)
        {
            _isEnd = true;
            _textMeshProList.ForEach(text => text.DOFade(0, _popUpFade));
            _spriteRendererList.ForEach(renderer => renderer.DOFade(0, _popUpFade));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}

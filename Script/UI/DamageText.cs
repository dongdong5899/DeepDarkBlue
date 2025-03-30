using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : DestroyLifetime
{
    [SerializeField] private TextMeshPro _textMeshPro;

    [SerializeField] private float _speed;
    [SerializeField] private float _xWavePower;
    [SerializeField] private float _xWaveSpeed;
    [SerializeField] private ParticleSystem _popEffect;

    public void Init(int damage, float size)
    {
        _textMeshPro.fontSize = size;
        _textMeshPro.text = damage.ToString();
        _textMeshPro.transform.localPosition = new Vector3(-_xWavePower, 0, 0);
        _textMeshPro.transform.DOLocalMoveX(_xWavePower, _xWaveSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        Spawn();
    }

    protected override void Update()
    {
        base.Update();
        transform.position += Vector3.up * Time.deltaTime * _speed;
        _textMeshPro.alpha = Mathf.Clamp(1 - CurrentLifetime / (lifetime + 0.6f), 0, 1);
    }

    public override void DelayDie()
    {
        _textMeshPro.transform.DOKill();
        Instantiate(_popEffect, _textMeshPro.transform.position, Quaternion.identity);
        base.DelayDie();
    }
}

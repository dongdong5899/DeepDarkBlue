using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashbangProjectile : DestroyLifetime
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Light2D _light2D;
    private float _speed;
    private int _damage;
    private LayerMask _whatIsTarget;

    private Tween _lightBoomTween;

    public void Init(float speed, Vector3 direction, int damage, LayerMask whatIsTarget)
    {
        _speed = speed;
        transform.right = direction;
        _damage = damage;
        _whatIsTarget = whatIsTarget;
    }

    private void FixedUpdate()
    {
        if (_isDead) return;

        Vector2 movement = transform.right * Time.fixedDeltaTime * _speed;
        transform.position += (Vector3)movement;

        _spriteRenderer.transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 10f * Time.fixedDeltaTime));
    }

    public override void Die()
    {
        AudioManager.Instance.PlaySound(EAudioName.FlashBang, transform, transform.position);
        _spriteRenderer.enabled = false;
        float startIntensity = 1.5f;
        _lightBoomTween = DOTween.To(() => startIntensity, value => _light2D.intensity = value, 0, dieDelay).SetEase(Ease.InCirc);
        base.Die();
    }

    public override void DelayDie()
    {
        if (_lightBoomTween != null && _lightBoomTween.IsActive()) _lightBoomTween.Kill();
        base.DelayDie();
    }
}

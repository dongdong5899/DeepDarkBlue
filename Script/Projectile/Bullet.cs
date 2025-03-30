using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : DestroyLifetime
{
    [SerializeField] private BetweenBoxCollider2D _projectileCollider;
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
        if (_projectileCollider.CheckCollision(_whatIsTarget, out RaycastHit2D[] hits, movement))
        {
            if (hits[0].transform.TryGetComponent(out MonsterBase monsterBase))
            {
                CameraManager.Instance.ShakeCamera(8, 8, 0.2f);
                monsterBase.TakeDamage(_damage);
            }
            Die();
            transform.position += transform.right * hits[0].distance;
        }
        else
        {
            transform.position += (Vector3)movement;
        }
    }

    public override void Die()
    {
        _spriteRenderer.enabled = false;
        float startIntensity = _light2D.pointLightOuterRadius * 2.5f;
        _lightBoomTween = DOTween.To(() => startIntensity, value => _light2D.pointLightOuterRadius = value, 0, dieDelay);
        base.Die();
    }

    public override void DelayDie()
    {
        if (_lightBoomTween != null && _lightBoomTween.IsActive()) _lightBoomTween.Kill();
        base.DelayDie();
    }
}

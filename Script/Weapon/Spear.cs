using DG.Tweening;
using UnityEngine;

public class Spear : Weapon
{
    private Vector3 _startPos;
    [SerializeField] private BetweenBoxCollider2D _projectileCollider;
    [SerializeField] private LayerMask _whatIsTarget;
    [SerializeField] private float _radius;
    [SerializeField] private int _damage;
    [SerializeField] private ParticleSystem _pierceEffect;

    private void Awake()
    {
        _startPos = transform.localPosition;
    }

    public override bool Use()
    {
        AudioManager.Instance.PlaySound(EAudioName.WeaponAttack, transform. transform.position);
        CameraManager.Instance.ShakeCamera(5, 5, 0.3f);
        Instantiate(_pierceEffect, transform.position, transform.rotation);

        Vector2 movement = _startPos + Vector3.right * _radius;
        if (_projectileCollider.CheckCollision(_whatIsTarget, out RaycastHit2D[] hits, movement))
        {
            if (hits[0].transform.TryGetComponent(out MonsterBase monsterBase))
            {
                CameraManager.Instance.ShakeCamera(8, 8, 0.2f);
                monsterBase.TakeDamage(_damage);
                return true;
            }
        }
        transform.localPosition = movement;
        transform.DOKill();
        transform.DOLocalMove(_startPos, 0.3f).SetEase(Ease.OutCirc);
        return false;
    }
}

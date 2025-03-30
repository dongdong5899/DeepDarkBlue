using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] private ParticleSystem _shootEffect;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _bulletDamage;
    [SerializeField] private Transform _firePos;
    [SerializeField] private LayerMask _whatIsTarget;

    public override bool Use()
    {
        AudioManager.Instance.PlaySound(EAudioName.Bullet, GameManager.Instance.PlayerTrm, GameManager.Instance.PlayerTrm.position);
        CameraManager.Instance.ShakeCamera(6, 6, 0.2f);
        Transform shootEffect = Instantiate(_shootEffect, _firePos.position, Quaternion.identity).transform;
        shootEffect.right = transform.right;
        Bullet bullet = Instantiate(_bullet, _firePos.position, Quaternion.identity);
        bullet.Init(_bulletSpeed, transform.right, _bulletDamage, _whatIsTarget);

        return true;
    }
}

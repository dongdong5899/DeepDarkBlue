using UnityEngine;

public class Flashbang : Weapon
{
    [SerializeField] private FlashbangProjectile _flashbangProjectile;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private LayerMask _whatIsTarget;

    public override bool Use()
    {
        FlashbangProjectile flashbangProjectile = Instantiate(_flashbangProjectile, transform.position, Quaternion.identity);
        flashbangProjectile.Init(_speed, transform.right, _damage, _whatIsTarget);

        return true;
    }
}

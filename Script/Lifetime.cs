using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Lifetime : MonoBehaviour
{
    public float lifetime;
    public float randomLifetimeInterval;
    public float dieDelay;
    public float CurrentLifetime { get; private set; }
    public float CurrentDelayDietime { get; private set; }
    protected bool _isDead;
    protected bool _isDelayDead;

    public void Spawn()
    {
        CurrentLifetime = 0;
        CurrentDelayDietime = 0;
        lifetime += Random.Range(-randomLifetimeInterval / 2, randomLifetimeInterval / 2);
        _isDead = false;
        _isDelayDead = false;
    }

    protected virtual void Update()
    {
        if (_isDelayDead) return;
        CurrentLifetime += Time.deltaTime;
        if (CurrentLifetime > lifetime)
        {
            if (_isDead == false) Die();
            CurrentDelayDietime += Time.deltaTime;
            if (CurrentDelayDietime > dieDelay)
            {
                _isDelayDead = true;
                DelayDie();
            }
        }
    }

    public virtual void Die()
    {
        if (_isDead) return;
        _isDead = true;
        CurrentLifetime += lifetime;
    }
    public abstract void DelayDie();
}

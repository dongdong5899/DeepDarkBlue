using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class GlowFish : MonsterBase
{
    [SerializeField] private Transform core;
    [SerializeField] private float notificationRange;
    private bool _isNoticed;
    private bool _coroutineStarted;
    private Transform _tempPos;
    private IEnumerator _currentRoutine;
    [SerializeField] private int dir;
    [SerializeField] private LayerMask player;
    private Collider2D _target;
    //[SerializeField] private Light2D light;
    [SerializeField] private SpriteRenderer _sprite;

    public bool IsNoticed => _isNoticed;

    protected override void Initilalize()
    {
        _sprite.color = Color.white;
        _isNoticed = false;
        currentHp = maxHp;
        state = MonsterState.Idle;
        _coroutineStarted = false;
        
    }

    private void FixedUpdate()
    {
        if (_target != null) return;

        _target = Physics2D.OverlapCircle(core.position, notificationRange, player);
        if (_target is not null&&!_isNoticed)
        {
            _sprite.color = Color.red;
            _isNoticed = true;
            state = MonsterState.Move;
        }

    }

    void Start()
    {
        Initilalize();
    }
    protected override void OnStateIdle()
    {
        if (!_isNoticed && !_coroutineStarted)
        {
            Flip();
            dir *= -1;

            _coroutineStarted = true;
            _currentRoutine = Patrol();
            StartCoroutine(_currentRoutine);
        }
    }

    private void Flip()
    {
        var scale = core.localScale;
        scale.x *= -1;
        core.localScale = scale;
    }

    private void FallowFlip()
    {
        if (core.transform.position.x-_target.transform.position.x > 0.2f)
        {
            var scale = core.localScale;
            scale.x = -1;
            core.localScale = scale;
        }
        else
        {
            var scale = core.localScale;
            scale.x = 1;
            core.localScale = scale;
        }
    }
    private IEnumerator Patrol()
    {
        for (var i = 0f; i <= 2f; i += Time.deltaTime)
        {
            var o = core;
            var position = o.transform.position;
            position = new Vector2(position.x + moveSpeed * Time.deltaTime*dir,
                position.y);
            o.transform.position = position;
            yield return null;
        }
        _coroutineStarted = false;
    }

    protected override void OnStateMove()
    {
        if(_currentRoutine != null)
        {
            StopCoroutine(_currentRoutine);
        }
        FallowFlip();
        core.transform.position = Vector2.MoveTowards(core.transform.position, _target.transform.position,moveSpeed*Time.deltaTime);
    }

    protected override void OnStateAttack()
    {
        Debug.Log("Attack");
    }
    protected override void OnStateDie()
    {
        if (isDead) return;

        Fade(0, 0.6f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
        isDead = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(core.position, notificationRange);
    }
}

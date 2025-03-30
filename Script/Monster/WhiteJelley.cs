// # System
using System.Collections;

// # Unity
using UnityEngine;
using DG.Tweening;

public class WhiteJelley : MonsterBase
{
    [Header("�ٿ ������ �� ����")]
    [SerializeField]
    private float     bounceDelay;
    private float     curBounceDelay;
    [SerializeField]
    private float     bounceSpeed;

    [Header("���� ���� �� ����")]
    [SerializeField]
    private float     moveDuration;

    [Header("���� ���� �� ����")]
    [SerializeField]
    private float     attackDuration;
    [SerializeField]
    private float     attackCooltime;

    [Header("�÷��̾� ���� �� ����")]
    [SerializeField]
    private float     detectRange;
    [SerializeField]
    private LayerMask layerMask;

    private Coroutine homeCancleCoroutine;

    private bool      isAttackCooltime;
    private bool      isDetectPlayer;
    private bool      isHome;
    private bool      isAttack;
    private bool      isUp;

    private Vector3   defaultPosition;

    private void Start()
    {
        Initilalize();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        isDetectPlayer = Physics2D.OverlapCircle(transform.position, detectRange, layerMask);
    }

    protected override void Initilalize()
    {
        defaultPosition = transform.position;

        currentHp = maxHp;
        isUp      = true;
    }

    protected override void OnStateAttack()
    {
        if (isAttack)         return;
        if (isAttackCooltime) return;

        Debug.Log("����");

        if(homeCancleCoroutine != null)
        {
            StopCoroutine(homeCancleCoroutine);
            homeCancleCoroutine = null;
        }

        isAttack = true;

        transform.position = GameManager.Instance.Player.transform.position;
        GameManager.Instance.Player.GrabStateToggle(true);
        GameManager.Instance.Player.TakeDamage(attackPower);

        StartCoroutine(AttackPlayerCoroutine());
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

    protected override void OnStateIdle()
    {
        if (isHome) return;
        
        if(isDetectPlayer && !isAttackCooltime)
        {
            state = MonsterState.Move;
            return;
        }

        if(Quaternion.Angle(Quaternion.Euler(0, 0, 0), transform.rotation) != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        MoveUpDown();
    }

    protected override void OnStateMove()
    {
        if(homeCancleCoroutine == null)
        {
            homeCancleCoroutine = StartCoroutine(nameof(CancelMoveToPlayer));
        }

        Vector3 direction = (GameManager.Instance.Player.transform.position - transform.position).normalized;

        // ������ 
        transform.position += direction * moveSpeed * Time.deltaTime;

        // ��ǥ�� ���� ȸ��
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetAngle      -= 90f;

        float smoothedAngle = Mathf.LerpAngle(transform.localEulerAngles.z, targetAngle, 5f * Time.deltaTime);
        transform.rotation  = Quaternion.Euler(0, 0, smoothedAngle);

        if(Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position) < 0.1f + (moveSpeed * 0.2f))
        {
            state = MonsterState.Attack;
        }
    }

    private void MoveUpDown()
    {
        curBounceDelay += Time.deltaTime;
        if (curBounceDelay >= bounceDelay)
        {
            isUp = (isUp == true) ? false : true;
            curBounceDelay = 0;
        }

        if (isUp)
        {
            transform.position += Vector3.up * bounceSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.down * bounceSpeed * Time.deltaTime;
        }
    }

    private IEnumerator AttackPlayerCoroutine()
    {
        isAttackCooltime = true;

        yield return new WaitForSeconds(attackDuration);
        GameManager.Instance.Player.GrabStateToggle(false);

        state = MonsterState.Idle;

        isHome   = true;
        isAttack = false;
        StartCoroutine(GoHomeCoroutine());
    }

    private IEnumerator WaitAttackCoolTimeCoroutine()
    {
        yield return new WaitForSeconds(attackCooltime);
        isAttackCooltime = false;
    }

    private IEnumerator GoHomeCoroutine()
    {
        float   speed     = moveSpeed * 0.1f; 
        Vector3 direction = (defaultPosition - transform.position).normalized;

        while (Vector3.Distance(transform.position, defaultPosition) >= 0.1f)
        {
            // ��ǥ�� ���� ȸ��
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            targetAngle      -= 90f;

            float smoothedAngle = Mathf.LerpAngle(transform.localEulerAngles.z, targetAngle, 5f * Time.deltaTime);
            transform.rotation  = Quaternion.Euler(0, 0, smoothedAngle);

            transform.position += direction * moveSpeed * Time.deltaTime;

            yield return null;
        }

        isHome = false;
        StartCoroutine(WaitAttackCoolTimeCoroutine());
    }
    
    private IEnumerator CancelMoveToPlayer()
    {
        yield return new WaitForSeconds(moveDuration);

        isHome              = true;
        isAttackCooltime    = true;
        homeCancleCoroutine = null;

        StartCoroutine(GoHomeCoroutine());
        state = MonsterState.Idle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}

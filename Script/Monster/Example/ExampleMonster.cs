using UnityEngine;

public class ExampleMonster : MonsterBase
{
    private void Start()
    {
        Initilalize();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Initilalize()
    {
        maxHp = currentHp;
        Debug.Log("Init");
    }

    protected override void OnStateIdle()
    {
        Debug.Log("Idle");
    }

    protected override void OnStateMove()
    {
        Debug.Log("Move");
    }

    protected override void OnStateAttack()
    {
        Debug.Log("Attack");
    }

    protected override void OnStateDie()
    {
        if (isDead) return; // 중복 실행 방지 

        isDead = true;
    }
}

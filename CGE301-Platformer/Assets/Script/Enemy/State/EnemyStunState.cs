using UnityEngine;

public class EnemyStunState : EnemyState
{
    private EnemyAIController enemyAi;
    private float stunTimer;

    public EnemyStunState(EnemyAIController enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
        enemyAi = enemy;
    }

    public void SetStunDuration(float duration)
    {
        stunTimer = Mathf.Max(0f, duration);
    }

    public override void Enter()
    {
        enemyAi.StopMove();
    }

    public override void Tick()
    {
        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0f)
        {
            stateMachine.ChangeState(enemyAi.PatrolState);
        }
    }

    public override void Exit()
    {
    }
}

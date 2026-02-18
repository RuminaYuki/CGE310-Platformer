using UnityEngine;

public class EnemyAlertState : EnemyState
{
    private EnemyAIController enemyAi;
    private float timer;

    public EnemyAlertState(EnemyAIController enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
        enemyAi = enemy;
    }

    public override void Enter()
    {
        timer = enemyAi.AlertDuration;
        enemyAi.StopMove();
    }

    public override void Tick()
    {
        timer -= Time.deltaTime;
        if (timer > 0f)
        {
            return;
        }

        if (enemyAi.Player == null)
        {
            stateMachine.ChangeState(enemyAi.PatrolState);
            return;
        }

        stateMachine.ChangeState(enemyAi.AttackState);
    }

    public override void Exit()
    {
        enemyAi.StopMove();
    }
}

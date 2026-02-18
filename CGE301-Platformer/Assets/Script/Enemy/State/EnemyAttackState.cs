using UnityEngine;
public class EnemyAttackState : EnemyState
{
    private EnemyAIController enemyAi;

    public EnemyAttackState(EnemyAIController enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
        enemyAi = enemy;
    }

    public override void Enter()
    {
        
    }

    public override void Tick()
    {
        if (enemyAi.Player == null || !enemyAi.CanSeePlayer() || enemyAi.IsAtPlatformEdge())
        {
            stateMachine.ChangeState(enemyAi.PatrolState);
            return;
        }

        if (enemyAi.IsPlayerInAttackRange())
        {
            enemyAi.StopMove();
            enemyAi.TryAttackPlayer();
            return;
        }
        enemyAi.MoveTowardPlayer();
    }

    public override void Exit()
    {
        enemyAi.StopMove();
    }
}

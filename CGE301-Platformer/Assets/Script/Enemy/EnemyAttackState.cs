public class EnemyAttackState : EnemyState
{
    private EnemyAIController enemyAi;
    private bool hasStartedAttackCycle;

    public EnemyAttackState(EnemyAIController enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
        enemyAi = enemy;
    }

    public override void Enter()
    {
        hasStartedAttackCycle = false;
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
            if (!hasStartedAttackCycle)
            {
                enemyAi.StartAttackCooldown();
                hasStartedAttackCycle = true;
                return;
            }

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

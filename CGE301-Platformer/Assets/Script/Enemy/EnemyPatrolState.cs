public class EnemyPatrolState : EnemyState
{
    private EnemyAIController enemyAi;

    public EnemyPatrolState(EnemyAIController enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine)
    {
        enemyAi = enemy;
    }

    public override void Tick()
    {
        if (enemyAi.CanSeePlayer())
        {
            enemyAi.StopMove();
            stateMachine.ChangeState(enemyAi.AlertState);
            return;
        }

        if (enemyAi.IsAtPlatformEdge())
        {
            enemyAi.StopMove();
            enemyAi.ReversePatrolDirection();
            return;
        }

        enemyAi.MoveInPatrolDirection();
    }

    public override void Exit()
    {
        enemyAi.StopMove();
    }
}

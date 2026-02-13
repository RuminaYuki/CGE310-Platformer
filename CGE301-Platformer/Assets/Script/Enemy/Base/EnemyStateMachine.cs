public class EnemyStateMachine
{
    public EnemyState CurrentState { get; private set; }

    public void Initialize(EnemyState startState)
    {
        CurrentState = startState;
        CurrentState?.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        if (newState == null)
        {
            return;
        }

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}

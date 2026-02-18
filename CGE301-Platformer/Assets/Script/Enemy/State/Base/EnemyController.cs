using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    protected Rigidbody2D rb;

    protected EnemyStateMachine stateMachine;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new EnemyStateMachine();
    }

    protected virtual void Start()
    {
        // Initialize your first state here, e.g.
        // stateMachine.Initialize(new EnemyIdleState(this, stateMachine));
    }

    protected virtual void Update()
    {
        stateMachine.CurrentState?.Tick();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.CurrentState?.FixedTick();
    }

    public void ChangeState(EnemyState newState)
    {
        stateMachine.ChangeState(newState);
    }
}

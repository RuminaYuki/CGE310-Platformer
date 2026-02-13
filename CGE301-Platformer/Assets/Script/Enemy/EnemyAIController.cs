using UnityEngine;

public class EnemyAIController : EnemyController
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;

    [Header("Patrol")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckFront;
    [SerializeField] private float raycastDistance = 0.3f;

    [Header("Alert")]
    [SerializeField] private float alertDuration = 0.5f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackInterval = 1f;
    [SerializeField] private int attackDamage = 1;

    [Header("Line Of View")]
    [SerializeField] private Transform viewPoint;
    [SerializeField, Range(1f, 10f)] private float viewDistance = 4f;
    [SerializeField, Range(1f, 179f)] private float viewAngle = 60f;

    private int patrolDirection = 1;
    private float nextAttackTime;

    private EnemyPatrolState patrolState;
    private EnemyAlertState alertState;
    private EnemyAttackState attackState;

    protected override void Awake()
    {
        base.Awake();
        patrolDirection = transform.localScale.x < 0f ? -1 : 1;

        patrolState = new EnemyPatrolState(this, stateMachine);
        alertState = new EnemyAlertState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(patrolState);
    }

    public EnemyState PatrolState => patrolState;
    public EnemyState AlertState => alertState;
    public EnemyState AttackState => attackState;
    public float AlertDuration => alertDuration;

    public Transform Player
    {
        get
        {
            if (player == null)
            {
                GameObject found = GameObject.FindGameObjectWithTag("Player");
                if (found != null)
                {
                    player = found.transform;
                }
            }

            return player;
        }
    }

    public void MoveInPatrolDirection()
    {
        SetFacing(patrolDirection);
        rb.linearVelocity = new Vector2(patrolDirection * moveSpeed, rb.linearVelocity.y);
    }

    public void ReversePatrolDirection()
    {
        SetFacing(-patrolDirection);
    }

    public bool IsAtPlatformEdge()
    {
        if (groundLayer.value == 0)
        {
            return false;
        }

        Vector2 origin = groundCheckFront != null
            ? (Vector2)groundCheckFront.position
            : (Vector2)transform.position + new Vector2(0.4f * patrolDirection, 0f);

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, raycastDistance, groundLayer);
        return hit.collider == null;
    }

    public bool CanSeePlayer()
    {
        if (Player == null)
        {
            return false;
        }

        Vector2 origin = viewPoint != null ? (Vector2)viewPoint.position : (Vector2)transform.position;
        Vector2 toPlayer = (Vector2)Player.position - origin;
        float distanceToPlayer = toPlayer.magnitude;
        if (distanceToPlayer > viewDistance || distanceToPlayer <= 0.01f)
        {
            return false;
        }

        float coneHalfAngle = viewAngle * 0.5f;
        Vector2 forward = Vector2.right * patrolDirection;
        if (Vector2.Angle(forward, toPlayer) > coneHalfAngle)
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.Raycast(origin, toPlayer.normalized, distanceToPlayer);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    public bool IsPlayerInAttackRange()
    {
        if (Player == null)
        {
            return false;
        }
        return Vector2.Distance(transform.position, Player.position) <= attackRange;
    }

    public void MoveTowardPlayer()
    {
        if (Player == null)
        {
            StopMove();
            return;
        }

        float direction = Mathf.Sign(Player.position.x - transform.position.x);
        if (Mathf.Abs(direction) > 0.01f)
        {
            SetFacing(direction > 0f ? 1 : -1);
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    public void TryAttackPlayer()
    {
        if (Player == null || !IsPlayerInAttackRange())
        {
            return;
        }

        if (Time.time < nextAttackTime)
        {
            Debug.Log(nextAttackTime);
            return;
        }

        nextAttackTime = Time.time + attackInterval;
        Debug.Log("Attacked");
    }

    public void StartAttackCooldown()
    {
        nextAttackTime = Time.time + attackInterval;
    }

    public void StopMove()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 groundOrigin = groundCheckFront != null
            ? groundCheckFront.position
            : transform.position + new Vector3(0.4f * patrolDirection, 0f, 0f);
        Gizmos.DrawLine(groundOrigin, groundOrigin + Vector3.down * raycastDistance);

        Gizmos.color = Color.yellow;
        Vector3 viewOrigin = viewPoint != null ? viewPoint.position : transform.position;
        float coneHalfAngle = viewAngle * 0.5f;
        Vector3 forward = Vector3.right * patrolDirection;
        Vector3 leftEdge = Quaternion.Euler(0f, 0f, coneHalfAngle) * forward;
        Vector3 rightEdge = Quaternion.Euler(0f, 0f, -coneHalfAngle) * forward;
        Gizmos.DrawLine(viewOrigin, viewOrigin + forward * viewDistance);
        Gizmos.DrawLine(viewOrigin, viewOrigin + leftEdge * viewDistance);
        Gizmos.DrawLine(viewOrigin, viewOrigin + rightEdge * viewDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void SetFacing(int direction)
    {
        patrolDirection = direction >= 0 ? 1 : -1;
        Vector3 currentScale = transform.localScale;
        currentScale.x = Mathf.Abs(currentScale.x) * patrolDirection;
        transform.localScale = currentScale;
    }

}

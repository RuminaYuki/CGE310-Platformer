using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    InputSystem_Actions playerController;
    Collider2D col;
    Facing2D facing;
    SpriteRenderer spriteRenderer;

    InputAction move;
    InputAction jump;
    InputAction dash;
    Vector2 moveDirection;

    [Header("Walk")]
    [SerializeField] float moveSpeed = 5f;
    public bool isKnockback = false;

    [Header("Jump")]
    [SerializeField] float jumpForce = 8f;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 12f;
    [SerializeField] float dashDuration = 0.15f;
    [SerializeField] float dashCooldown = 0.5f;
    bool isDashing;
    float dashTimer;
    float nextDashTime;
    int dashDirection = 1;

    [Header("Ground Check")]
    [SerializeField] GroundCheck groundCheck;

    [Header("Wall Check")]
    [SerializeField] float distanceCheck = 0.3f;
    [SerializeField] LayerMask layer;
    bool isGrounded;
    bool jumpQueued;

    Vector2 origin;
    Vector2 size;

    public Rigidbody2D Rb => rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        facing = new Facing2D(transform, spriteRenderer);
        playerController = new InputSystem_Actions();
        if (groundCheck == null)
        {
            Debug.Log("dont have groundCheck");
            return;
        }
    }

    void OnEnable()
    {
        move = playerController.Player.Move;
        move.Enable();

        jump = playerController.Player.Jump;
        jump.Enable();

        dash = playerController.Player.Dash;
        dash.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        jump.Disable();

        dash.Disable();
    }

    void Update()
    {
        ReadInputForWalk();
        QueueJump();
        QueueDash();
    }

    void FixedUpdate()
    {
        if (isKnockback)
        {
            return;
        }

        if (isDashing)
        {
            ApplyDash();
            return;
        }

        Walk();
        ApplyJump();
    }

    void QueueJump()
    {
        isGrounded = groundCheck != null && groundCheck.IsGrounded();

        if (jump.WasPressedThisFrame())
        {
            jumpQueued = true;
        }
    }

    void ReadInputForWalk()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    void QueueDash()
    {
        if (dash == null || !dash.WasPressedThisFrame())
        {
            return;
        }

        if (Time.time < nextDashTime)
        {
            return;
        }

        float inputX = moveDirection.x;
        if (Mathf.Abs(inputX) > 0.01f)
        {
            dashDirection = inputX > 0f ? 1 : -1;
        }
        else
        {
            dashDirection = facing != null ? facing.FacingDirection : 1;
        }

        isDashing = true;
        dashTimer = dashDuration;
        nextDashTime = Time.time + dashCooldown;
    }

    void Walk()
    {
        float inputX = moveDirection.x;
        if (facing != null)
        {
            facing.SetFacingByDirectionX(inputX);
        }

        int facingDirection = facing != null ? facing.FacingDirection : 1;
        bool isTouchingWall = IsTouchingWall(facingDirection);
        if (isTouchingWall && Mathf.Abs(inputX) > 0.01f)
        {
            SetHorizontalVelocity(0f);
            return;
        }

        SetHorizontalVelocity(inputX * moveSpeed);
    }

    void ApplyDash()
    {
        if (IsTouchingWall(dashDirection))
        {
            EndDash();
            SetHorizontalVelocity(0f);
            return;
        }

        SetHorizontalVelocity(dashDirection * dashSpeed);
        dashTimer -= Time.fixedDeltaTime;

        if (dashTimer <= 0f)
        {
            EndDash();
        }
    }

    void EndDash()
    {
        isDashing = false;
    }

    bool IsTouchingWall(int facingDirection)
    {
        Vector2 castDirection = Vector2.right * facingDirection;
        origin = new Vector2(
            facingDirection > 0 ? col.bounds.max.x : col.bounds.min.x,
            col.bounds.center.y
        );
        size = new Vector2(0.001f, col.bounds.size.y - 0.1f);

        return Physics2D.BoxCast(origin, size, 0f, castDirection, distanceCheck, layer);
    }

    void SetHorizontalVelocity(float x)
    {
        rb.linearVelocity = new Vector2(x, rb.linearVelocity.y);
    }

    void ApplyJump()
    {
        if (jumpQueued && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        jumpQueued = false;
    }

    void OnDrawGizmosSelected()
    {
        Collider2D targetCol = col != null ? col : GetComponent<Collider2D>();
        if (targetCol == null)
        {
            return;
        }

        int facingDirection = facing != null ? facing.FacingDirection : 1;
        Vector2 drawOrigin = new Vector2(
            facingDirection > 0 ? targetCol.bounds.max.x : targetCol.bounds.min.x,
            targetCol.bounds.center.y
        );
        Vector2 drawSize = new Vector2(0.001f, targetCol.bounds.size.y - 0.1f);

        Vector3 origin3 = new Vector3(drawOrigin.x, drawOrigin.y, 0f);
        Vector3 size3 = new Vector3(drawSize.x, drawSize.y, 0f);
        Vector3 end = origin3 + Vector3.right * facingDirection * distanceCheck;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(origin3, size3);
        Gizmos.DrawWireCube(end, size3);
        Gizmos.DrawLine(origin3 + new Vector3(0f, size3.y * 0.5f, 0f), end + new Vector3(0f, size3.y * 0.5f, 0f));
        Gizmos.DrawLine(origin3 - new Vector3(0f, size3.y * 0.5f, 0f), end - new Vector3(0f, size3.y * 0.5f, 0f));

        Gizmos.color = Color.white;
        Gizmos.DrawLine(origin3, end);
    }
}

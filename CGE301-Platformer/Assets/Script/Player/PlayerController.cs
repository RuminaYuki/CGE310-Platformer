using UnityEngine;
using UnityEngine.EventSystems;
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
    Vector2 moveDirection;

    [Header("Walk")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 8f;

    [Header("Ground Check")]
    [SerializeField] GroundCheck groundCheck;

    [Header("Wall Check")]
    [SerializeField] float distanceCheck = 0.3f;
    bool isGrounded;
    bool jumpQueued;

    Vector2 origin;
    Vector2 size;

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

    private void OnEnable()
    {
        move = playerController.Player.Move;
        move.Enable();

        jump = playerController.Player.Jump;
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    void Update()
    {
        ReadInputForWalk();
        QueueJump();
    }

    void FixedUpdate()
    {
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

    void Walk()
    {   
        //สลับฝั่ง
        float inputX = moveDirection.x;
        if (facing != null)
        {
            facing.SetFacingByDirectionX(inputX);
        }

        int facingDirection = facing != null ? facing.FacingDirection : 1;
        Vector2 castDirection = Vector2.right * facingDirection;
        // เช็กกำแพง
        origin = new Vector2(
            facingDirection > 0 ? col.bounds.max.x : col.bounds.min.x,
            col.bounds.center.y
        );
        size = new Vector2(0.001f, col.bounds.size.y - 0.1f);

        bool isTouchingWall = Physics2D.BoxCast(origin, size, 0f, castDirection, distanceCheck);
        if (isTouchingWall && Mathf.Abs(inputX) > 0.01f)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }
        // เดินปกติ
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
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

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    InputSystem_Actions playerController;
    
    InputAction move;
    InputAction jump;
    Vector2 moveDirection;

    [Header("Walk")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Ground Check")]
    [SerializeField] GroundCheck groundCheck;

    [Header("Jump")]
    [SerializeField] float jumpForce = 8f;
    bool isGrounded;
    bool jumpQueued;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = new InputSystem_Actions();
        if(groundCheck == null)
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
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }
    void ApplyJump()
    {
        if (jumpQueued && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        jumpQueued = false;
    }

}

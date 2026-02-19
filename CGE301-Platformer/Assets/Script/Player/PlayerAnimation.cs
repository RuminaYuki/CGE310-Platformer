using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    PlayerController playerController;
    void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(playerController.Rb.linearVelocity.x));
        animator.SetFloat("yVelocity",playerController.Rb.linearVelocity.y);
        if (playerController.IsGrounded)
        {
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }

        if (playerController.JumpPressedThisFrame)
        {
            animator.SetTrigger("jump");
        }

        if(playerController.isKnockback)
        {
            animator.SetBool("isKnockback", true);
        }
        else
        {
            animator.SetBool("isKnockback", false);
        }

        if (playerController.IsDashing)
        {
            animator.SetBool("isDashing", true);
        }
        else
        {
            animator.SetBool("isDashing", false);
        }
    }
}

using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] float groundRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position,groundRadius,groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,groundRadius);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class PlayerOneWayPlatform : MonoBehaviour
{
    private GameObject currentOneWayPlatform;
    private InputSystem_Actions playerInputActions;
    private InputAction down;
    private Collider2D playerCollider;

    [SerializeField] float disalbeTime = 0.75f;

    private void Awake()
    {
        playerInputActions = new InputSystem_Actions();
        playerCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        down = playerInputActions.Player.Down;
        down.Enable();
    }

    private void OnDisable()
    {
        down.Disable();
    }

    private void Update()
    {
        if (down.WasPressedThisFrame())
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        Collider2D platformCollider = currentOneWayPlatform.GetComponent<Collider2D>();
        if (platformCollider == null || playerCollider == null)
        {
            yield break;
        }

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(disalbeTime);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}

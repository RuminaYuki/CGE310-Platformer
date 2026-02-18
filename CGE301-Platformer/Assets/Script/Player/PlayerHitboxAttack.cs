using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerHitbox : MonoBehaviour
{
    private Collider2D col;
    private InputAction attack;
    private InputSystem_Actions playerController;

    [SerializeField] private float stunDuration = 1f;
    private float hitboxActiveDuration = 0.2f;
    [SerializeField] private float attackCooldown = 2f;

    private float nextAttackTime;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        playerController = new InputSystem_Actions();
    }

    void Start()
    {
        col.enabled = false;
    }

    void OnEnable()
    {
        attack = playerController.Player.Attack;
        attack.Enable();
    }

    void OnDisable()
    {
        attack.Disable();
    }

    void Update()
    {
        if (!attack.WasPressedThisFrame())
        {
            return;
        }

        if (Time.time < nextAttackTime)
        {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;
        StartCoroutine(EnableHitbox(hitboxActiveDuration));
    }

    IEnumerator EnableHitbox(float duration)
    {
        col.enabled = true;
        yield return new WaitForSeconds(duration);
        col.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        Debug.Log("Hit Enemy");
        other.gameObject.GetComponent<EnemyDamageHeadler>().StunReceiver(stunDuration);
    }
}

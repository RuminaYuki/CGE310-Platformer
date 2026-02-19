using UnityEngine;

public class EnemyHitboxAttack : MonoBehaviour
{
    [SerializeField] float KnockbackForce;
    [SerializeField] float KnockbackDuration = 1f;

    private EnemyAIController enemyAIController;
    private Transform enemyTransform;

    void Awake()
    {
        enemyAIController = GetComponentInParent<EnemyAIController>();
        enemyTransform = enemyAIController != null ? enemyAIController.transform : transform.root;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        PlayerDamageHeadler damageHandler = collision.gameObject.GetComponent<PlayerDamageHeadler>();
        if (damageHandler == null) return;

        damageHandler.KnockBackReceiver(KnockbackForce, KnockbackDuration, enemyTransform);
    }
}

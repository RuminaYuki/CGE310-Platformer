using UnityEngine;

public class EnemyHitboxAttack : MonoBehaviour
{
    [SerializeField] float KnockbackForce;
    [SerializeField] float KnockbackDuration = 1f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player")) return;
        collision.gameObject.GetComponent<PlayerDamageHeadler>().KnockBackReceiver(KnockbackForce,KnockbackDuration,transform);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("Player")) return;
        collision.gameObject.GetComponent<PlayerDamageHeadler>().KnockBackReceiver(KnockbackForce,KnockbackDuration,transform);
    }
}

using System.Collections;
using UnityEngine;

public class PlayerDamageHeadler : MonoBehaviour
{
    private PlayerController playerController;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void KnockBackReceiver(float knockFroce,float duration, Transform enemy)
    {
        Vector2 direction = (transform.position - enemy.transform.position).normalized;
        StartCoroutine(KnockBackCoolDown(duration));
        playerController.Rb.linearVelocity = direction * knockFroce;
    }

    IEnumerator KnockBackCoolDown(float duration)
    {
        playerController.isKnockback = true;
        yield return new WaitForSeconds(duration);
        playerController.isKnockback = false;
    }
}

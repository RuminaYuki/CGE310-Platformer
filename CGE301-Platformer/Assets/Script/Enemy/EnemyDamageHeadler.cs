using UnityEngine;

public class EnemyDamageHeadler : MonoBehaviour
{
    private EnemyAIController enemyAI;

    void Awake()
    {
        enemyAI = GetComponentInParent<EnemyAIController>();
    }

    public void StunReceiver(float duration)
    {
        EnemyStunState stunState = enemyAI.StunState as EnemyStunState;
        if (stunState == null)
        {
            return;
        }

        stunState.SetStunDuration(duration);
        enemyAI.ChangeState(stunState);
    }
}

using UnityEngine;

public class EnableHitbox : MonoBehaviour
{
    [SerializeField] Collider2D hitbox;

    public void Enable()
    {
        hitbox.enabled = true;
    }
    public void Disable()
    {
        hitbox.enabled = false;
    }
}

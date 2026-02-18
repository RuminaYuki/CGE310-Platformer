using UnityEngine;
using UnityEngine.Events;

public class Items : MonoBehaviour
{
    public bool destroyAfterActive;
    public UnityEvent _onTriger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _onTriger.Invoke();
            if (destroyAfterActive)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

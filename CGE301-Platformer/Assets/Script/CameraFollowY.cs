using UnityEngine;

public class CameraFollowY : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private Vector3 offset;

    private float velocityY;
    private float fixedX;
    private float fixedZ;

    private void Awake()
    {
        fixedX = transform.position.x;
        fixedZ = transform.position.z;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        float desiredY = target.position.y + offset.y;
        float nextY = Mathf.SmoothDamp(transform.position.y, desiredY, ref velocityY, smoothTime);

        transform.position = new Vector3(
            fixedX + offset.x,
            nextY,
            fixedZ + offset.z
        );
    }
}

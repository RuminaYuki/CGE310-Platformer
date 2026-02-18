using UnityEngine;

public class ItemsMove : MonoBehaviour
{
    public float speed = 2f;      // ความเร็วในการขยับ
    public float height = 0.5f;   // ระยะขึ้นลง

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}

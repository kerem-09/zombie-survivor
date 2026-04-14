using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;

    public float minX = -20f, maxX = 20f;
    public float minY = -12f, maxY = 12f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

        desired.x = Mathf.Clamp(desired.x, minX, maxX);
        desired.y = Mathf.Clamp(desired.y, minY, maxY);

        transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);
    }
}

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;           // drag Player here
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0f, 2f, -10f); // note the -10

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desired = target.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desired, smoothSpeed);
        // keep Z fixed so it stays at -10
        transform.position = new Vector3(smoothed.x, smoothed.y, offset.z);
    }
}

using UnityEngine;

public class MetroidvaniaCamera2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Dead Zone")]
    [SerializeField] private float deadZoneWidth = 2f;
    [SerializeField] private float deadZoneHeight = 1.5f;

    [Header("Follow")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Bounds")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds = new Vector2(-10f, -10f);
    [SerializeField] private Vector2 maxBounds = new Vector2(10f, 10f);

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            // keep z so the camera does not go to 0
            transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
            return;
        }

        Vector3 camPos = transform.position;
        Vector3 targetPos = target.position;

        float halfDZWidth = deadZoneWidth * 0.5f;
        float halfDZHeight = deadZoneHeight * 0.5f;

        if (targetPos.x < camPos.x - halfDZWidth)
            camPos.x = targetPos.x + halfDZWidth;
        else if (targetPos.x > camPos.x + halfDZWidth)
            camPos.x = targetPos.x - halfDZWidth;

        if (targetPos.y < camPos.y - halfDZHeight)
            camPos.y = targetPos.y + halfDZHeight;
        else if (targetPos.y > camPos.y + halfDZHeight)
            camPos.y = targetPos.y - halfDZHeight;

        Vector3 desired = camPos + offset;

        if (useBounds && cam != null)
        {
            float vertExtent = cam.orthographicSize;
            float horzExtent = vertExtent * cam.aspect;

            desired.x = Mathf.Clamp(desired.x, minBounds.x + horzExtent, maxBounds.x - horzExtent);
            desired.y = Mathf.Clamp(desired.y, minBounds.y + vertExtent, maxBounds.y - vertExtent);
        }

        // always force z so we never look from inside the scene
        desired.z = -10f;

        transform.position = Vector3.Lerp(transform.position, desired, followSpeed * Time.deltaTime);
    }
}

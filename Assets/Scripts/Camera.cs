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

    // these store default and active offsets
    private Vector3 defaultOffset;
    private Vector3 currentOffset;

    // for zoom functionality
    private float defaultOrthoSize;
    private Coroutine zoomRoutine;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        defaultOffset = offset;
        currentOffset = offset;

        if (cam != null)
        {
            defaultOrthoSize = cam.orthographicSize;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
            return;
        }

        Vector3 camPos = transform.position;
        Vector3 targetPos = target.position;

        float halfDZWidth = deadZoneWidth * 0.5f;
        float halfDZHeight = deadZoneHeight * 0.5f;

        // camera movement on X
        if (targetPos.x < camPos.x - halfDZWidth)
            camPos.x = targetPos.x + halfDZWidth;
        else if (targetPos.x > camPos.x + halfDZWidth)
            camPos.x = targetPos.x - halfDZWidth;

        // camera movement on Y
        if (targetPos.y < camPos.y - halfDZHeight)
            camPos.y = targetPos.y + halfDZHeight;
        else if (targetPos.y > camPos.y + halfDZHeight)
            camPos.y = targetPos.y - halfDZHeight;

        Vector3 desired = camPos + currentOffset;

        if (useBounds && cam != null)
        {
            float vertExtent = cam.orthographicSize;
            float horzExtent = vertExtent * cam.aspect;

            desired.x = Mathf.Clamp(desired.x, minBounds.x + horzExtent, maxBounds.x - horzExtent);
            desired.y = Mathf.Clamp(desired.y, minBounds.y + vertExtent, maxBounds.y - vertExtent);
        }

        // force Z depth
        desired.z = -10f;

        transform.position = Vector3.Lerp(transform.position, desired, followSpeed * Time.deltaTime);
    }

    // called externally from zones
    public void SetOffset(Vector3 newOffset)
    {
        currentOffset = newOffset;
    }

    public void ResetOffset()
    {
        currentOffset = defaultOffset;
    }

    //--------------------------------------------------------------
    // ZOOM FUNCTIONS
    //--------------------------------------------------------------

    public void SetZoom(float targetSize, float duration)
    {
        if (cam == null) return;

        if (zoomRoutine != null)
            StopCoroutine(zoomRoutine);

        zoomRoutine = StartCoroutine(ZoomCoroutine(targetSize, duration));
    }

    public void ResetZoom(float duration)
    {
        SetZoom(defaultOrthoSize, duration);
    }

    private System.Collections.IEnumerator ZoomCoroutine(float targetSize, float duration)
    {
        float startSize = cam.orthographicSize;
        float timeElapsed = 0f;
        duration = Mathf.Max(0.01f, duration);

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;

            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        cam.orthographicSize = targetSize;
    }
}

using UnityEngine;

public class CameraOffsetZone : MonoBehaviour
{
    [Header("Zoom Settings")]
    [Tooltip("The zoomed out orthographic size while in this zone.")]
    [SerializeField] private float zoneOrthoSize = 7f;

    [SerializeField] private float zoomTransitionTime = 0.5f;

    [Tooltip("Optional: drag the camera here, otherwise it will auto-find.")]
    [SerializeField] private MetroidvaniaCamera2D camFollow;

    private void Start()
    {
        if (camFollow == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
                camFollow = mainCam.GetComponent<MetroidvaniaCamera2D>();
        }

        if (camFollow == null)
        {
            camFollow = FindFirstObjectByType<MetroidvaniaCamera2D>();
        }

        if (camFollow == null)
        {
            Debug.LogError("CameraOffsetZone: Could not find MetroidvaniaCamera2D.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("CameraOffsetZone: OnTriggerEnter2D with " + other.name);

        if (camFollow == null) return;
        if (!other.CompareTag("Player")) return;

        Debug.Log("CameraOffsetZone: Player entered, zooming out");
        camFollow.SetZoom(zoneOrthoSize, zoomTransitionTime);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("CameraOffsetZone: OnTriggerExit2D with " + other.name);

        if (camFollow == null) return;
        if (!other.CompareTag("Player")) return;

        Debug.Log("CameraOffsetZone: Player exited, resetting zoom");
        camFollow.ResetZoom(zoomTransitionTime);
    }
}

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;      // The object to look at
    public float zoomSpeed = 5f;  // Speed for zooming
    public float minZoom = 2f;    // Minimum zoom distance
    public float maxZoom = 20f;   // Maximum zoom distance

    private Camera cam;
    private float distanceToTarget = 10f; // Default distance from camera to target

    void Start()
    {
        cam = Camera.main;
        if (target != null)
        {
            // Set initial distance based on current position
            distanceToTarget = Vector3.Distance(transform.position, target.position);
        }
    }

    void Update()
    {
        HandleZoom();
        LookAtTarget();
    }

    void LookAtTarget()
    {
        if (target != null)
        {
            transform.LookAt(target);

            // Keep camera at the correct distance from the target
            Vector3 dir = (transform.position - target.position).normalized;
            transform.position = target.position + dir * distanceToTarget;
        }
    }

    void HandleZoom()
    {
        // Mouse scroll wheel (desktop)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            distanceToTarget = Mathf.Clamp(distanceToTarget - scroll * zoomSpeed, minZoom, maxZoom);
        }

        // Pinch to zoom (mobile)
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (prevTouch0 - prevTouch1).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            distanceToTarget = Mathf.Clamp(distanceToTarget - difference * 0.01f * zoomSpeed, minZoom, maxZoom);
        }
    }
}

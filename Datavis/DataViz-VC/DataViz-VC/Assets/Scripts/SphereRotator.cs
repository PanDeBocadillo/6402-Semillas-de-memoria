using UnityEngine;

public class SphereRotator : MonoBehaviour
{
    private Vector3 lastMousePosition;
    private bool isDragging = false;
    public float rotationSpeed = 5f; // Adjust the speed of rotation

    void Update()
    {
        // Detect mouse drag
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            // Rotate the sphere based on mouse movement
            float rotationX = delta.y * rotationSpeed * Time.deltaTime;
            float rotationY = -delta.x * rotationSpeed * Time.deltaTime;

            transform.Rotate(Vector3.right, rotationX, Space.World);
            transform.Rotate(Vector3.up, rotationY, Space.World);

            lastMousePosition = Input.mousePosition;
        }
    }
}

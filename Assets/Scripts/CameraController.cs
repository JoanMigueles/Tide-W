using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float panSpeed = 0.1f;

    public float minZoomDistance = 10f;
    public float maxZoomDistance = 74f;

    public float minX = -50f;
    public float maxX = 50f;
    public float minZ = -50f;
    public float maxZ = 50f;

    private void Update()
    {
        // Zooming with mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll);

        // Panning (dragging) on the x-z plane
        if (Input.GetMouseButton(1)) 
        {
            float deltaX = Input.GetAxis("Mouse X");
            float deltaY = Input.GetAxis("Mouse Y");
            PanCamera(deltaX, deltaY);
        }
    }

    void ZoomCamera(float scroll)
    {
        // Calculate the new position based on the zoom input
        Vector3 newPosition = transform.position + transform.forward * scroll * zoomSpeed;

        // Calculate the distance from the camera to the target point
        float distance = Vector3.Distance(newPosition, Vector3.zero);

        // Clamp the distance to the zoom limits
        if (distance >= minZoomDistance && distance <= maxZoomDistance)
        {
            transform.position = newPosition;
        }
    }

    void PanCamera(float deltaX, float deltaY)
    {
        // Convert mouse movement to world space movement
        Vector3 pan = new Vector3(deltaX, 0, deltaY);

        // Translate the camera's position based on the mouse movement
        Vector3 newPosition = transform.position - pan * panSpeed;

        // Clamp the new position to the defined boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

        // Update the camera's position
        transform.position = newPosition;
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float panSpeed = 0.1f;

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
        // Adjust the camera's position or field of view based on the mouse wheel input
        transform.position += transform.forward * scroll * zoomSpeed;
    }

    void PanCamera(float deltaX, float deltaY)
    {
        // Convert mouse movement to world space movement
        Vector3 pan = new Vector3(deltaX, 0, deltaY);

        // Translate the camera's position based on the mouse movement, but constrain it to the x-z plane
        transform.Translate(-pan * panSpeed, Space.World);
    }
}

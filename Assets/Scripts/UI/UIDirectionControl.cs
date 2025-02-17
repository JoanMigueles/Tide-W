using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool useRelativeRotation = true;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (useRelativeRotation)
        {
            // Get the direction from the health bar to the camera
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;
            directionToCamera.x = 0; // Ignore the x-axis

            // Create a rotation that looks at the camera
            Quaternion lookRotation = Quaternion.LookRotation(-directionToCamera);

            // Apply the rotation to the transform
            transform.rotation = lookRotation;
        }
    }
}
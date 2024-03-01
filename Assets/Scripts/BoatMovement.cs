using System.Collections;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    // Speed of boat movement
    public float moveSpeed = 5f;

    // Reference to the hexagon manager
    public Hexagon hexagon;

    // Flag to check if boat is moving
    private bool isMoving = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonUp(0) && !isMoving)
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If the ray hits a collider
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the collider is a hexagon
                if (hit.collider.CompareTag("Hex"))
                {
                    GameObject targetHexagon = hit.collider.gameObject;

                    // Check if the target hexagon is a neighbor
                    if (hexagon.IsNeighbor(targetHexagon))
                    {
                        Debug.Log("going");
                        // Move the boat to the center of the target hexagon
                        StartCoroutine(MoveToTarget(targetHexagon.transform.position));
                        hexagon = targetHexagon.GetComponent<Hexagon>();
                        // Calculate direction vector to the target hexagon
                        Vector3 direction = (targetHexagon.transform.position - transform.position).normalized;

                        // Rotate the boat to face the direction it's moving
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                    else
                    {
                        Debug.Log("Selected hexagon is not a neighbor.");
                    }
                }
            }
        }
    }

    // Move boat to the target position
    IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        isMoving = true;

        // Keep moving towards the target position until reached
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}
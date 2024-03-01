using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexagon : MonoBehaviour
{

    // Reference to the image component
    private Image image;

    // Original color of the image
    private Color originalColor;

    // Color to use when the hexagon is at full opacity
    public Color fullOpacityColor;

    // Start is called before the first frame update
    void Start()
    {
        // Get the image component
        image = GetComponentInChildren<Image>();

        // Store the original color of the image
        originalColor = image.color;
    }

    // Called when the mouse enters the hexagon collider
    private void OnMouseEnter()
    {
        // Set the color to full opacity when hovered over
        image.color = fullOpacityColor;
    }

    // Called when the mouse exits the hexagon collider
    private void OnMouseExit()
    {
        // Restore the original color when not hovered over
        image.color = originalColor;
    }

    // Check if the given hexagon is a neighbor of this hexagon
    public bool IsNeighbor(GameObject hexagon)
    {
        // Calculate the position difference between this hexagon and the target hexagon
        Vector3 diff = hexagon.transform.position - transform.position;

        Debug.Log(diff);
        // Define the range within which the hexagons are considered neighbors
        float neighborRange = 11f; // Adjust this value as needed

        // Check if the target hexagon is within the neighbor range
        if (Mathf.Abs(diff.x) < neighborRange && Mathf.Abs(diff.z) < neighborRange)
        {
            // Ensure that the hexagons are not the same
            if (hexagon != gameObject)
            {
                return true;
            }
        }

        return false;
    }
}
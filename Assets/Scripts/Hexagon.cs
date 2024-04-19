using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexagon : MonoBehaviour
{

    private Image image;
    private Color originalColor;
    public Color fullOpacityColor;

    void Start()
    {
        image = GetComponentInChildren<Image>();
        originalColor = image.color;
    }

    private void OnMouseEnter()
    {
        image.color = fullOpacityColor;
    }

    private void OnMouseExit()
    {
        image.color = originalColor;
    }

    public bool IsNeighbor(GameObject hexagon)
    {
        Vector3 diff = hexagon.transform.position - transform.position;

        float neighborRange = 11f;

        if (Mathf.Abs(diff.x) < neighborRange && Mathf.Abs(diff.z) < neighborRange)
        {
            if (hexagon != gameObject)
            {
                return true;
            }
        }

        return false;
    }
}
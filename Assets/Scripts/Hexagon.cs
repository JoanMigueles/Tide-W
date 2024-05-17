using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexagon : MonoBehaviour
{

    private Image image;
    private Color originalColor;
    [SerializeField] private bool available = true;
    public Color fullOpacityColor;

    void Start()
    {
        image = GetComponentInChildren<Image>();
        originalColor = image.color;
    }

    private void Update()
    {
        // Check if the mouse is over the hexagon
        if (IsMouseOverHexagon())
        {
            OnMouseEnter();
        }
        else
        {
            OnMouseExit();
        }
    }

    private bool IsMouseOverHexagon()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast and check if it hits a collider in the Hexagon layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("UI")))
        {
            Hexagon hexagon = hit.collider.GetComponent<Hexagon>();
            if (hexagon == this) return true;
        }

        return false;
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

    public void SetAvailability(bool available)
    {
        this.available = available;
    }

    public bool IsFree()
    {
        return available;
    }
}
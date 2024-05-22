using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Side
{
    None, 
    Ally,
    Enemy
}
public class Hexagon : MonoBehaviour
{

    private Image image;
    private Color originalColor;
    private Color fullOpacityColor;

    public Side side;

    [SerializeField] private bool available = true;
    [SerializeField] private Collider hexCollider;
    

    void Start()
    {
        image = GetComponentInChildren<Image>();
        if (side == Side.Ally) fullOpacityColor = new Color(1f, 1f, 1f, 1f);
        else fullOpacityColor = new Color(1f, 1f, 1f, 0.5f);

        originalColor = new Color(fullOpacityColor.r, fullOpacityColor.g, fullOpacityColor.b, 0.5f);
    }

    private void Update()
    {
        if (available)
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
        } else
        {
            image.color = Color.yellow;
        }
        
    }

    private bool IsMouseOverHexagon()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast and check if it hits a collider in the Hexagon layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("UI")))
        {
            if (hexCollider == hit.collider) return true;
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
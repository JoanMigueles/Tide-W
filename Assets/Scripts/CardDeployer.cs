using UnityEngine;
using UnityEngine.EventSystems;

public class CardDeployer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject boatPrefab;

    private GameObject cardBeingDragged;

    private void Start()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.instance.setDragging(true);
        // Instantiate the card being dragged
        cardBeingDragged = Instantiate(gameObject, gameObject.transform);
        cardBeingDragged.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the card being dragged
        cardBeingDragged.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        // Check if the card was dropped onto the hexboard
        RaycastHit hit;
        if (Physics.Raycast(eventData.pointerCurrentRaycast.worldPosition, Vector3.down, out hit))
        {
            Hexagon hexagon = hit.collider.GetComponent<Hexagon>();
            if (hexagon != null)
            {
                // Instantiate the prefab in the hexagon's position
                InstantiatePrefabInHex(hexagon);
            }
        }

        // Destroy the dragged card object
        Destroy(cardBeingDragged);
        GameManager.instance.setDragging(false);
    }

    private void InstantiatePrefabInHex(Hexagon hexagon)
    {
        // Instantiate your prefab in the hexagon's position
        // For example:
        GameObject boat = Instantiate(boatPrefab, hexagon.transform.position, Quaternion.identity);
        boat.GetComponent<BoatMovement>().hexagon = hexagon;

    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class CardDeployer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject unitPrefab; // Reference to the 3D asset prefab

    private RectTransform cardRectTransform;
    private CanvasGroup canvasGroup;

    private Vector3 initialPosition;
    private Vector2 offset;

    private void Start()
    {
        cardRectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = cardRectTransform.position;
        Debug.Log(initialPosition);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }

        // Calculate offset from pointer position to the center of the card
        offset = new Vector2(cardRectTransform.position.x, cardRectTransform.position.y) - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update card position relative to pointer position
        cardRectTransform.position = eventData.position + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Hexagon hexagon = hit.collider.GetComponent<Hexagon>();
            if (hexagon != null)
            {
                // Instantiate the unit prefab onto the hexagon
                GameObject unit = Instantiate(unitPrefab, hexagon.transform.position, Quaternion.identity);
                BoatMovement boatMovement = unit.GetComponent<BoatMovement>();
                if (boatMovement != null ) {
                    boatMovement.hexagon = hexagon;
                }
                // If successful, make the card disappear
                gameObject.SetActive(false);
                return;
            }
        }

        // If deployment fails, return the card to its initial position before the drag
        cardRectTransform.position = initialPosition;
    }
}
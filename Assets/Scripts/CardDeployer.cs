using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDeployer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Entity troopPrefab; // Reference to the 3D asset prefab
    private AudioSource audioSource;
    [SerializeField] private AudioClip grabClip;
    [SerializeField] private AudioClip dropClip;
    [SerializeField] private AudioClip notAllowedClip;

    private Vector2 offset;
    private Vector2 initialPosition;

    private Transform canvas;
    private Transform cardSlot;

    private void Start()
    {
        canvas = transform.parent.parent.parent;
        cardSlot = transform.parent;
        audioSource = GameManager.instance.GetComponent<AudioSource>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        audioSource.PlayOneShot(grabClip);
        // Calculate offset from pointer position to the center of the card
        offset = new Vector2(transform.position.x, transform.position.y) - eventData.position;
        initialPosition = transform.position;
        transform.SetParent(canvas);
        GameManager.instance.SetDragging(true);
    }

    public void OnDrag(PointerEventData eventData)
    {

        Image image = GetComponent<Image>();
        if (eventData.position.y > 100f)
        {
            image.color = new Color(1f, 1f, 1f, 0.2f);
        } else
        {
            image.color = new Color(1f, 1f, 1f, 1f);
        }
        // Update card position relative to pointer position
        transform.position = eventData.position + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Look for the hexagon to instantiate the boat
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("UI")))
        {
            Hexagon hexagon = hit.collider.GetComponentInParent<Hexagon>();
            float currentOrbs = GameManager.instance.GetOrbs();

            if (hexagon != null && currentOrbs >= troopPrefab.orbCost && hexagon.IsFree() && hexagon.side == Side.Ally)
            {
                audioSource.PlayOneShot(dropClip);
                // Instantiate the unit prefab onto the hexagon
                GameManager.instance.SetOrbs(currentOrbs - troopPrefab.orbCost);

                Entity entity = Instantiate(troopPrefab, hexagon.transform.position, Quaternion.identity);

                // Check if the instantiated entity is a Structure
                if (entity is Structure structure)
                {
                    // Set the hexagon property for the structure
                    structure.hexagon = hexagon;
                    hexagon.SetAvailability(false);
                }

                // If successful, make the card disappear
                Destroy(gameObject);

                GameManager.instance.RestockCard(troopPrefab.type);
                GameManager.instance.SetDragging(false);
                return;
            }
        }

        // If deployment fails, return the card to its initial position before the drag
        audioSource.PlayOneShot(notAllowedClip);
        transform.position = initialPosition;
        transform.SetParent(cardSlot);
        Image image = GetComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 1f);

        GameManager.instance.SetDragging(false);
    }
}
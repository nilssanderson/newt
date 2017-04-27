using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    /* Variables ------------------------------------------------------------ */
	public Item item;
	public int amount;
    public int slot;
    
    private Inventory inv;
    private Vector2 offset;


	/* Functions ------------------------------------------------------------ */
    void Start() {
        inv = GameObject.Find("Inventory").GetComponent<Inventory>(); // Get Inventory component on Inventory GameObject
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (item != null) { // Make sure there is an item
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y); // Offset by size of item
            this.transform.SetParent(this.transform.parent.parent); // Set the items parent to the slotPanel, to render on top of the slots
			this.transform.position = eventData.position; // Set the position of this item to the mouse position
            GetComponent<CanvasGroup>().blocksRaycasts = false; // Disable raycasts
		}
    }

    public void OnDrag(PointerEventData eventData) {
        if (item != null) { // Make sure there is an item
			this.transform.position = eventData.position - offset; // Set the position of this item to the mouse position minus item size
		}
    }

    public void OnEndDrag(PointerEventData eventData) {
        this.transform.SetParent(inv.slots[slot].transform); // Grab transform of current slot
        this.transform.position = inv.slots[slot].transform.position; // Set position of item to position of current slot
        GetComponent<CanvasGroup>().blocksRaycasts = true; // Enable raycasts
    }
}

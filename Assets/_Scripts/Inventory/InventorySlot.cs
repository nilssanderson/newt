using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler {

	/* Functions ------------------------------------------------------------ */
	public int id;
	private Inventory inv;


	/* Functions ------------------------------------------------------------ */
	void Start() {
		inv = GameObject.Find("Inventory").GetComponent<Inventory>(); // Get Inventory component on Inventory GameObject
	}

	public void OnDrop(PointerEventData eventData) {
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>(); // Set droppedItem to 

		if (inv.items[id].ID == -1) {
			inv.items[droppedItem.slot] = new Item(); // Empty out the old slot
			inv.items[id] = droppedItem.item; // Set new slot to the item
			droppedItem.slot = id; // Set slot of item we dragged to the current slot
		} else {
			Transform item = this.transform.GetChild(0); // Get first item
			item.GetComponent<ItemData>().slot = droppedItem.slot; // Slot that was here is now the new slot
			item.transform.SetParent(inv.slots[droppedItem.slot].transform);
			item.transform.position = inv.slots[droppedItem.slot].transform.position;

			droppedItem.slot = id;
			droppedItem.transform.SetParent(this.transform);
			droppedItem.transform.position = this.transform.position;

			inv.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
			inv.items[id] = droppedItem.item;
		}
    }
}

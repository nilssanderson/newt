using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	/* Variables ------------------------------------------------------------ */
	GameObject inventoryPanel;
	GameObject slotPanel;
	ItemDatabase database;
	public GameObject inventorySlot;
	public GameObject inventoryItem;

	private int slotAmount = 20;

	public List<Item> items = new List<Item>();
	public List<GameObject> slots = new List<GameObject>();


	/* Functions ------------------------------------------------------------ */
	void Start() {
		database = GetComponent<ItemDatabase>();
		inventoryPanel = GameObject.Find("Inventory Panel");
		slotPanel = inventoryPanel.transform.FindChild("Slot Panel").gameObject;

		// Add slots and set parent to the slotPanel
		for (int i = 0; i < slotAmount; i++) {
			items.Add(new Item()); // Create empty item with id of -1
			slots.Add(Instantiate(inventorySlot)); // Add slot to inventorySlot
			slots[i].GetComponent<InventorySlot>().id = i; // Set slots id
			slots[i].transform.SetParent(slotPanel.transform); // Set slots parent to slotPanel
		}

		AddItem(0);
		AddItem(1);
		AddItem(1);
		AddItem(1);
		AddItem(1);
		AddItem(1);
	}

	public void AddItem(int _id) {
		Item itemToAdd = database.FetchItemByID(_id); // Get item by id

		if (itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd)) { // If item is stackable and if there is an item in the slot
			for (int i = 0; i < items.Count; i++) {
				if (items[i].ID == _id) {
					ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>(); // Get child of slot and get the ItemData component
					data.amount++; // Increment the amount
					data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString(); // Update the text child to the amount
					break; // Once added break the loop
				}
			}
		} else {
			for (int i = 0; i < items.Count; i++) {
				if (items[i].ID == -1) { // If item slot is empty
					items[i] = itemToAdd; // Add item to the empty item slot
					GameObject itemObject = Instantiate(inventoryItem); // Create item
					itemObject.GetComponent<ItemData>().item = itemToAdd; // Set item to itemToAdd
					itemObject.GetComponent<ItemData>().slot = i; // Set slot to inventory slot int
					itemObject.transform.SetParent(slots[i].transform); // Set parent to the empty item parent slot
					itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite; // Add items sprite to the item
					itemObject.transform.position = Vector2.zero; // Set position to 0,0 of item slot
					itemObject.name = itemToAdd.Title; // Set the name of the item in hierarchy
					break; // Once found break the loop
				}
			}
		}
	}

	bool CheckIfItemIsInInventory(Item _item) {
		for (int i = 0; i < items.Count; i++) {
			if (items[i].ID == _item.ID) {
				return true;
			}
		}
		return false;
	}
}

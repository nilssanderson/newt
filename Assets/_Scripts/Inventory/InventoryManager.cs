using UnityEngine;

public class InventoryManager : MonoBehaviour {

	/* Functions ------------------------------------------------------------ */
	public bool inventoryOpen = false;
	public GameObject inventory;
	private GameObject inventoryInstance;


	/* Functions ------------------------------------------------------------ */
	void Start() {
		inventoryInstance = (GameObject)Instantiate(inventory);
		// inventoryInstance.GetComponent<Canvas>().enabled = false;
		// inventoryInstance.SetActive(false);
	}

	void Update() {
		if (Input.GetKeyUp("i")) {
			if (!inventoryOpen) {
				inventoryOpen = true;
				// inventoryInstance.GetComponent<Canvas>().enabled = true;
				inventoryInstance.SetActive(true);
			} else {
				inventoryOpen = false;
				// inventoryInstance.GetComponent<Canvas>().enabled = false;
				inventoryInstance.SetActive(false);
			}
		}
	}
}

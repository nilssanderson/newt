using UnityEngine;

public class AddsItem : MonoBehaviour {

	public Inventory inv;

	void Start() {
		GameObject temp;
		temp = GameObject.Find("Player");
		inv = temp.GetComponent<InventoryManager>().inventory.GetComponent<Inventory>();
	}

	void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.tag == "Player") {
			inv.AddItem(1);
			Destroy(gameObject);
		}
    }
}

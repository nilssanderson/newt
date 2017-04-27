﻿using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour {

	/* Variables
	***********/

	[SerializeField]
	Behaviour[] componentsToDisable;

	private Camera sceneCamera;

	[SerializeField]
	private string remoteLayerName = "RemotePlayer";

	[SerializeField]
	private string dontDrawLayerName = "DontDraw";

	[SerializeField]
	private GameObject playerGraphics;

	[SerializeField]
	private GameObject playerUIPrefab;
	private GameObject playerUIInstance;

	/* Functions
	***********/

	void Start() {

		if (!isLocalPlayer) {

			DisableComponents ();
			AssignRemoteLayer ();
			
		} else {

			sceneCamera = Camera.main;

			if (sceneCamera != null) {
				sceneCamera.gameObject.SetActive (false);
			}

			// Disable player graphics for local player
			SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

			// Create PlayerUI
			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

			// Configure PlayerUI
			PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
			if (ui == null) {
				Debug.LogError("No PlayerUI component on PlayerUI prefab.");
			}
			ui.SetController(GetComponent<PlayerController>());
		}

		GetComponent<Player>().Setup();
	}

	void SetLayerRecursively(GameObject obj, int newLayer) {
		obj.layer = newLayer;

		foreach (Transform child in obj.transform) {
			SetLayerRecursively(child.gameObject, newLayer);
		}
	}

	public override void OnStartClient() {
		base.OnStartClient ();

		string _netID = GetComponent<NetworkIdentity> ().netId.ToString();
		Player _player = GetComponent<Player> ();
	
		GameManager.RegisterPlayer (_netID, _player);
	}

	void AssignRemoteLayer() {
		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}

	void DisableComponents() {

		// If we are not the local player
		// Loop through all the components in the array and disable them
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable [i].enabled = false;
		}
	}

	void OnDisable() {

		Destroy(playerUIInstance);

		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive (true);
		}

		GameManager.UnRegisterPlayer (transform.name);
	}

}

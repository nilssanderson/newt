﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	/* Variables
	***********/

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private bool invertCamera;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0;
	private float currentCameraRotationX;
	private Vector3 thrusterForce = Vector3.zero;

	[SerializeField]
	private float cameraRotationLimit = 85f;

	private Rigidbody rb;


	/* Functions
	***********/

	void Start() {
		rb = GetComponent<Rigidbody> ();
	}

	// Gets a movement vector
	public void Move (Vector3 _velocity) {

		velocity = _velocity;
	}

	// Gets a rotational vector
	public void Rotate (Vector3 _rotation) {

		rotation = _rotation;
	}

	// Gets a rotational vector for the camera
	public void RotateCamera (float _cameraRotationX) {

		cameraRotationX = _cameraRotationX;
	}

	// Gets a force vector for the thrusters
	public void ApplyThruster (Vector3 _thrusterForce) {

		thrusterForce = _thrusterForce;
	}

	// Run every physics iteration
	void FixedUpdate() {

		PerformMovement ();
		PerformRotation ();
	}

	// Perform movement based on velocity variable
	void PerformMovement() {

		if (velocity != Vector3.zero) {
			rb.MovePosition (rb.position + velocity * Time.fixedDeltaTime);
		}

		if (thrusterForce != Vector3.zero) {
			rb.AddForce (thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
		}
	}

	// Perform rotation
	void PerformRotation() {

		rb.MoveRotation (rb.rotation * Quaternion.Euler (rotation));

		if (cam != null) {

			// Set rotation and clamp it
			if (invertCamera) {
				currentCameraRotationX += cameraRotationX;
			} else {
				currentCameraRotationX -= cameraRotationX;
			}

			// Apply our rotation to the transform of the camera
			currentCameraRotationX = Mathf.Clamp (currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
			cam.transform.localEulerAngles = new Vector3 (currentCameraRotationX, 0, 0);
		}
	}
}

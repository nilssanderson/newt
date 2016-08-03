using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {


	/* Variables
	***********/

	[SerializeField]
	private float speed = 5f; // Allows a protected private field to show up in the inspector

	[SerializeField]
	private float lookSensitivity = 3f;

	[SerializeField]
	private float thrusterForce = 1000f;

	[Header ("Spring Settings:")]
	[SerializeField]
	private float jointSpring = 20f;
	[SerializeField]
	private float jointMaxForce = 40f;

	// Component caching
	private PlayerMotor motor;
	private ConfigurableJoint joint;
	private Animator animator;


	/* Functions
	***********/

	void Start() {

		motor = GetComponent<PlayerMotor> ();
		joint = GetComponent<ConfigurableJoint> ();
		animator = GetComponent<Animator> ();

		SetJointSettings (jointSpring);
	}

	void Update() {

		// Calculate movement velocity as a 3D vector
		float _xMove = Input.GetAxis("Horizontal");
		float _zMove = Input.GetAxis("Vertical");

		Vector3 _moveHorizontal = transform.right * _xMove;
		Vector3 _moveVertical = transform.forward * _zMove;

		// Final movement vector
		Vector3 _velocity = (_moveHorizontal + _moveVertical) * speed;

		// Animate movement
		animator.SetFloat("ForwardVelocity", _zMove);

		// Apply movement
		motor.Move(_velocity);

	// Player rotation
		// Calculate rotation as a 3D vector (turning around)
		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3 (0f, _yRot, 0f) * lookSensitivity;

		// Apply rotation
		motor.Rotate(_rotation);

	// Camera rotation
		// Calculate camera rotation as a 3D vector (turning around)
		float _xRot = Input.GetAxisRaw("Mouse Y");

		float _cameraRotationX = _xRot * lookSensitivity;

		// Apply camera rotation
		motor.RotateCamera(_cameraRotationX);

		// Calculate the thruster force based on player input
		Vector3 _thrusterForce = Vector3.zero;

		if (Input.GetButton ("Jump")) {
			_thrusterForce = Vector3.up * thrusterForce;
			SetJointSettings (0f);
		} else {
			SetJointSettings (jointSpring);
		}

		// Apply thruster force
		motor.ApplyThruster(_thrusterForce);
	}

	private void SetJointSettings( float _jointSpring) {
		joint.yDrive = new JointDrive {
			positionSpring = _jointSpring,
			maximumForce = jointMaxForce
		}; 
	}
}

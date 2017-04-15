using UnityEngine;
using System.Collections;

/// <summary>
/// Modified script from: http://wiki.unity3d.com/index.php?title=MouseOrbitImproved
/// </summary>
[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour {
	
	public static MouseOrbitImproved Instance;

	public Transform target;
	public LayerMask cameraCollisionLayer;
	public float wallDepenetration = .25f;
	public float distance = 5.0f;
	public float mouseXSpeed = 120.0f;
	public float mouseYSpeed = 120.0f;
	public float controllerXSpeed = 120.0f;
	public float controllerYSpeed = 120.0f;

	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	public float distanceMin = .5f;
	public float distanceMax = 15f;

	private Rigidbody rb;

	float x = 0.0f;
	float y = 0.0f;
	
	void Awake()
	{
		if(Instance == null){
			DontDestroyOnLoad (gameObject);
			Instance = this;
		}
		else if(Instance!=this)
			Destroy(gameObject);
	}

	// Use this for initialization
	void Start () 
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

		rb = GetComponent<Rigidbody>();

		// Make the rigid body not change rotation
		if (rb != null)
		{
			rb.freezeRotation = true;
		}
	}

	void LateUpdate () 
	{
		if (target) 
		{
			//RMB to move camera
			if(Input.GetMouseButton(1)){
				x += Input.GetAxis("Mouse X") * mouseXSpeed * Time.deltaTime;
				y -= Input.GetAxis("Mouse Y") * mouseYSpeed * Time.deltaTime;
			}
			x += Input.GetAxis("RHorizontal") * controllerXSpeed * Time.deltaTime;
			y += Input.GetAxis("RVertical") * controllerYSpeed * Time.deltaTime;
	

			y = ClampAngle(y, yMinLimit, yMaxLimit);
			Quaternion rotation = Quaternion.Euler(y, x, 0);

			//distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);
			distance = Mathf.Lerp( distanceMin, distanceMax,  (y - yMinLimit) /(yMaxLimit - yMinLimit));

			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;

			RaycastHit hit;
			if (Physics.Linecast (target.position, position, out hit, cameraCollisionLayer)) 
			{
				position = hit.point + (target.position - position).normalized * wallDepenetration;
			}

			transform.rotation = rotation;
			transform.position = position;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}
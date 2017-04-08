using UnityEngine;
using UnityEngine.UI;

public class WorldToScreenUI : MonoBehaviour {
	
	public Transform followTransform;
	public Vector3 offset;
	public Sprite image;
	public bool clampToScreen = false;
	public float lifespan = 3f;
	public Image childImage;
	// Use this for initialization
	void Start () {
		//Subscribe to event to happen after camera moves
		//Camera.main.GetComponent<> ().AfterMove += UpdatePosition;
		if(lifespan >= 0){
			Destroy (gameObject, lifespan);
		}
		if(childImage == null){
			childImage = GetComponentInChildren<Image>();
		}
	}
	void Update(){
	}
	void LateUpdate(){
		if(childImage.sprite != image){
			childImage.sprite = image;
			childImage.SetNativeSize();
		}
		UpdatePosition ();
	}

	// Update is called once per frame
	void UpdatePosition () {
		if(followTransform == null) return;
		float z = transform.position.z;
		Vector3 vec = Camera.main.WorldToScreenPoint(followTransform.position + offset);
		transform.position = Vector3.Scale(vec,new Vector3(1f,1f,0)) + new Vector3(0,0,z);

		//For unexpected behavior involving objects being behind the camera, still not technically correct.
		if(vec.z < 0){
			transform.position = Vector3.Scale(transform.position, new Vector3(-1f,-1f,1f));
		}

		if(clampToScreen){
			RectTransform rt = (RectTransform) transform;
			Vector2 size = rt.sizeDelta;
			float x = Mathf.Clamp(transform.position.x,size.x * rt.pivot.x,Screen.width - size.x * (1-rt.pivot.x));
			float y = Mathf.Clamp(transform.position.y,size.y * rt.pivot.y,Screen.height - size.y * (1-rt.pivot.y));
			transform.position = new Vector3(x,y,transform.position.z);
		}
	}
	void OnDestroy(){
		//Camera.main.GetComponent<IsometricCamera> ().AfterMove -= UpdatePosition;
	}
}

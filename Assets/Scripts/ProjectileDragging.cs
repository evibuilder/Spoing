using UnityEngine;

public class ProjectileDragging : MonoBehaviour {
	public float maxStretch = 3.0f;
	
	private SpringJoint2D spring;
	private Transform ball;
	private Ray rayToMouse;
	private float maxStretchSqr;
	private bool clickedOn;
	private Vector2 prevVelocity;
	
	
	void Awake () {
		spring = GetComponent <SpringJoint2D> ();
		ball = spring.connectedBody.transform;
	}
	
	void Start () {
		rayToMouse = new Ray(ball.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
	}
	
	void Update () {
		if (clickedOn)
			Dragging ();
		
		if (spring != null) {
			if (!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude) {
				Destroy (spring);
				GetComponent<Rigidbody2D>().velocity = prevVelocity;
			}
			
			if (!clickedOn)
				prevVelocity = GetComponent<Rigidbody2D>().velocity;
						
		} 
	}
	
	void OnMouseDown () {
		spring.enabled = false;
		clickedOn = true;
	}
	
	void OnMouseUp () {
		spring.enabled = true;
		GetComponent<Rigidbody2D>().isKinematic = false;
		clickedOn = false;
	}

	void Dragging () {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - ball.position;
		
		if (catapultToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
		}
		
		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}
}

using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	public Door _door;
	private SpringController spring;
	private BallController ball;
	// Use this for initialization
	void Start () {
		spring = GameObject.Find("spring").GetComponent<SpringController>();
		ball = GameObject.Find("ball").GetComponent<BallController>();
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D box)
	{
		if (box.name == "spring" || box.name == "ball") {
			Debug.Log ("Open Sesame");
			_door.OpenDoor ();
		} 

	}
}

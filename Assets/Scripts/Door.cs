using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public GameObject _door;
	private SpringController spring;
	private BallController ball;
	// Use this for initialization
	void Start () {
		
	}
	
	public void OpenDoor ()
	{
		_door.SetActive (false);
	}
}

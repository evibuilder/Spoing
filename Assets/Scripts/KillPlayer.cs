using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour {

	public LevelManager levelManager;
	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D box)
	{
		if (box.name == "spring") {
			levelManager.RespawnPlayer();
		} 
		else if (box.name == "ball") 
		{
			levelManager.RespawnPlayer();
		}
	}
		
}

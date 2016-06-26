using UnityEngine;
using System.Collections;

public class FinishLevel : MonoBehaviour {

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
		if (box.name == "spring" && box.name == "ball") {
			levelManager.FinishLevel();
		} 
	}
}

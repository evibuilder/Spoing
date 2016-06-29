using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject currentSpringPoint;
	public GameObject currentBallPoint;
	public GameManager gameManager;

	private SpringController spring;
	private BallController ball;
	// Use this for initialization
	void Start () {
		spring = FindObjectOfType<SpringController> ();
		ball = FindObjectOfType<BallController> ();
		gameManager = FindObjectOfType<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RespawnPlayer()
	{
		Debug.Log ("Dead thing");
		spring.transform.position = currentSpringPoint.transform.position;
		ball.transform.position = currentBallPoint.transform.position;
	}
	public void FinishLevel()
	{
		Debug.Log ("Level Finished");

	}
}

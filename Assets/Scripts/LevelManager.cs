using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;


public class LevelManager : MonoBehaviour {

	public GameObject currentSpringPoint;
	public GameObject currentBallPoint;
	public GameManager gameManager;
    public Text winText;
    public Text loseText;
    public Text LivesText;
    public int NumberOfLives = 3;

	private SpringController spring;
	private BallController ball;
    private Stopwatch timer;
    private int currentLives;

	// Use this for initialization
	void Start () {
		spring = FindObjectOfType<SpringController> ();
		ball = FindObjectOfType<BallController> ();
		gameManager = FindObjectOfType<GameManager> ();
        winText.enabled = false;
        loseText.enabled = false;

        currentLives = NumberOfLives;
        LivesText.enabled = true;
        LivesText.text = "Lives: " + currentLives.ToString();
        timer = new Stopwatch();
	}
	
	// Update is called once per frame
	void Update () {
	
        if(timer != null)
        {
            if(timer.ElapsedMilliseconds > 2000)
            {
                UnityEngine.Debug.Log("hello");
                gameManager.NextLevel();
            }
        }
	}

	public void RespawnPlayer()
	{
		spring.transform.position = currentSpringPoint.transform.position;
		ball.transform.position = currentBallPoint.transform.position;
	}
	public void FinishLevel()
	{
        winText.text = "Level Finished";
        winText.enabled = true;

        timer.Start();
	}

    public void GameOver()
    {
        loseText.text = "Level Failed";
        loseText.enabled = true;
        gameManager.RestartLevel();
    }

    public void Kill()
    {
        UnityEngine.Debug.Log("kill has been called");

        currentLives--;

        if (LivesText != null)
        {
            LivesText.text = "Lives: " + currentLives.ToString();
        }

        if (currentLives == 0)
            GameOver();
        else
            RespawnPlayer();
    }
}

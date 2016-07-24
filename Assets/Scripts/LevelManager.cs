using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;
using camera;


public class LevelManager : MonoBehaviour
{
    public GameManager gameManager;
    public Text winText;
    public Text loseText;
    public Text LivesText;
    public int NumberOfLives = 3;
    

    private Stopwatch timer;
    private int currentLives;
    private SpringController spring;
    private BallController ball;
    private CameraScript _camera;

    // Use this for initialization
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        winText.enabled = false;
        loseText.enabled = false;

        spring = GameObject.Find("spring").GetComponent<SpringController>();
        ball = GameObject.Find("ball").GetComponent<BallController>();
        _camera = GameObject.Find("Main Camera").GetComponent<CameraScript>();


        currentLives = NumberOfLives;
        LivesText.enabled = true;
        LivesText.text = "Lives: " + currentLives.ToString();
        timer = new Stopwatch();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (spring.IsActive())
            {
                if(spring.IsCharging() == false && spring.IsLaunching() == false)
                {
                    spring.SetActive(false);
                    ball.SetActive(true);
                    _camera.SwitchCamera();
                }
            }
            else if (ball.IsActive())
            {
                if(ball.IsSwinging() == false)
                {
                    ball.SetActive(false);
                    spring.SetActive(true);
                    _camera.SwitchCamera();
                }
            }
        }

        if (timer != null)
        {
            if (timer.ElapsedMilliseconds > 2000)
            {
                gameManager.NextLevel();
            }
        }
    }

    public void RespawnPlayer()
    {
        GameObject.Find("spring").GetComponent<Transform>().position = GameObject.Find("SpringRespawnPoint").GetComponent<Transform>().position;
        GameObject.Find("ball").GetComponent<Transform>().position = GameObject.Find("BallRespawnPoint").GetComponent<Transform>().position;
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

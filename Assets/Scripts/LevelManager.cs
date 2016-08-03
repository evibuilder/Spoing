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

    public Text TimeText;
    public Text FramesText;
    private Stopwatch nextLevelTimer;
    private Stopwatch levelTimer;
    private Stopwatch fps;
    private double FPScount;
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
        nextLevelTimer = new Stopwatch();
        levelTimer = new Stopwatch();
        levelTimer.Start();
        fps = new Stopwatch();
        FPScount = 0;

        fps.Start();
        TimeText.text = "Elapsed Time: "; 
        FramesText.text = "FPS: ";
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

        if (nextLevelTimer != null)
        {
            if (nextLevelTimer.ElapsedMilliseconds > 2000)
            {
                gameManager.NextLevel();
            }
        }
        TimeText.text = "Elapsed Time: " + (levelTimer.ElapsedMilliseconds/1000).ToString();

        UpdateFPS();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(850, 20, 100, 100), FramesText.text);
        GUI.Label(new Rect(850, 40, 200, 100), TimeText.text);
    }

    public void SetActive(string player)
    {
        if(player == "string")
        {
            spring.SetActive(true);
        }
        else if(player == "ball")
        {
            ball.SetActive(true);
        }
    }

    private void UpdateFPS()
    {
        FPScount++;

        if(fps.ElapsedMilliseconds > 1000)
        {
            double frames = System.Math.Round(fps.ElapsedMilliseconds / FPScount);


            FramesText.text = "FPS: " + frames.ToString();
            

            fps.Reset();
            fps.Start();
            FPScount = 0;
        }
    }

    public void RespawnPlayer()
    {
        GameObject.Find("spring").GetComponent<Transform>().position = GameObject.Find("SpringRespawnPoint").GetComponent<Transform>().position;
        GameObject.Find("ball").GetComponent<Transform>().position = GameObject.Find("BallRespawnPoint").GetComponent<Transform>().position;

        levelTimer.Reset();
        levelTimer.Start();
    }
    public void FinishLevel()
    {
        winText.text = "Level Finished with time of " + levelTimer.Elapsed.ToString();
        winText.enabled = true;

        levelTimer.Stop();
        nextLevelTimer.Start();
    }

    public void GameOver()
    {
        loseText.text = "Level Failed";
        loseText.enabled = true;

        levelTimer.Stop();

        gameManager.RestartLevel();
    }

    public void Kill()
    {
        UnityEngine.Debug.Log("kill has been called");

        currentLives--;
        levelTimer.Stop();

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

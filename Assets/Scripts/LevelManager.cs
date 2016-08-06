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
    public AudioClip win;
    public AudioClip die;
    public AudioClip fail;

    
    public Text TimeText;
    public Text FramesText;

    private AudioSource source;
    private Stopwatch nextLevelTimer;
    private Stopwatch levelTimer;
    private Stopwatch restartLevelTimer;
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

        source = GetComponent<AudioSource>();

        spring = GameObject.Find("spring").GetComponent<SpringController>();
        ball = GameObject.Find("ball").GetComponent<BallController>();
        _camera = GameObject.Find("Main Camera").GetComponent<CameraScript>();

        currentLives = NumberOfLives;
        LivesText.enabled = true;
        LivesText.text = "Lives: " + currentLives.ToString();
        nextLevelTimer = new Stopwatch();
        levelTimer = new Stopwatch();
        restartLevelTimer = new Stopwatch();
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
            if (nextLevelTimer.ElapsedMilliseconds > 5000)
            {
                gameManager.NextLevel();
            }
        }

        if(restartLevelTimer != null)
        {
            if(restartLevelTimer.ElapsedMilliseconds > 5000)
            {
                gameManager.RestartLevel();
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

        if(levelTimer != null)
        {
            levelTimer.Reset();
            levelTimer.Start();
        }
        
    }
    public void FinishLevel()
    {
        PlayWin();

        winText.enabled = true;
        winText.text = "Level Finished";

        levelTimer.Stop();
        nextLevelTimer.Start();
    }

    public void GameOver()
    {
        PlayFail();

        loseText.enabled = true;
        loseText.text = "Level Failed";

        levelTimer.Stop();

        restartLevelTimer.Start();
    }

    public void Kill()
    {
        UnityEngine.Debug.Log("kill has been called");

        PlayDeath();

        currentLives--;
        if(levelTimer != null)
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

    private float SetVolume()
    {
        float volLowRange = .5f;
        float volHighRange = 1.0f;

        return Random.Range(volLowRange, volHighRange);
    }

    public void PlayFail()
    {
        source.PlayOneShot(fail, SetVolume());
    }

    public void PlayDeath()
    {
        if(source != null)
            source.PlayOneShot(die, SetVolume());
    }

    public void PlayWin()
    {
        source.PlayOneShot(win, SetVolume());
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KillPlayer : MonoBehaviour
{

    public int numOfLives = 3;
    public LevelManager levelManager;
    public Text displayLives;

    private int currentLives;


    // Use this for initialization
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        currentLives = numOfLives;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Kill();
        }

        if(displayLives != null)
            displayLives.text = "Lives: " + currentLives.ToString();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "spring" || col.name == "ball")
        {
            Kill();
        }
    }

    public void Kill()
    {
        currentLives--;

        if(displayLives != null)
            displayLives.text = "Lives: " + currentLives.ToString();

        if (currentLives == 0)
            levelManager.GameOver();
        else
            levelManager.RespawnPlayer();
    }
}

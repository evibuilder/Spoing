using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KillPlayer : MonoBehaviour
{

    public LevelManager levelManager;

    private int currentLives;

    // Use this for initialization
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            levelManager.Kill();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(gameObject.name == "PuffSprite")
        {
            if (col.name == "spring" || col.name == "ball")
            {
                levelManager.Kill();
            }
        }
        else if(gameObject.name == "bug")
        {
            if (col.name == "spring")
            {
                levelManager.Kill();
            }
            else if(col.name == "ball")
            {
                bool falling = col.GetComponent<BallController>().IsFalling();
                bool launched = col.GetComponent<BallController>().IsLaunched();

                if(!falling && !launched)
                    levelManager.Kill();
            }
        }
    }
}

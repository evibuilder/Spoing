using UnityEngine;
using System.Collections;

public class FinishLevel : MonoBehaviour {

	public LevelManager levelManager;

    private bool springFinish;
    private bool ballFinish;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager>();
        springFinish = false;
        ballFinish = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (springFinish && ballFinish)
        {
            Debug.Log("finished has been called");
            levelManager.FinishLevel();
        }
    }

	void OnTriggerEnter2D(Collider2D box)
	{
		if (box.name == "spring") {
            Debug.Log("spring has finished");
            springFinish = true;
		} 
        else if(box.name == "ball")
        {
            Debug.Log("ball has finished");
            ballFinish = true;
        }
	}

    void OnTriggerStay2D(Collider2D box)
    {
        if (box.name == "spring")
        {
            Debug.Log("spring has finished");
            springFinish = true;
        }
        else if (box.name == "ball")
        {
            ballFinish = true;
        }
    }

    void OnTriggerExit2D(Collider2D box)
    {
        if (box.name == "spring")
        {
            Debug.Log("spring has exited");
            springFinish = false;
        }
        else if (box.name == "ball")
        {
            ballFinish = false;
        }
    }

   
}

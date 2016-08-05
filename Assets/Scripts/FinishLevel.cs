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
            levelManager.FinishLevel();
        }
    }

	void OnTriggerEnter2D(Collider2D box)
	{

		if (box.name == "spring") {
            springFinish = true;
		} 
        else if(box.name == "ball")
        {
            ballFinish = true;
        }
	}

    void OnTriggerStay2D(Collider2D box)
    {

        if (box.name == "spring")
        {
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
            springFinish = false;
        }
        else if (box.name == "ball")
        {
            ballFinish = false;
        }
    }
}

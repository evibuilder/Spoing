using UnityEngine;
using System.Collections;

public class PauseManagement : MonoBehaviour {

	public Transform canvas;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			Pause ();
		}

	}
	public void Pause()
	{
		if (canvas.gameObject.activeInHierarchy == false) 
		{
			canvas.gameObject.SetActive (true);
			Time.timeScale = 0;
		} 
		else 
		{
			canvas.gameObject.SetActive (false);
			Time.timeScale = 1;
		}
	}
	public void Exit ()
	{
		Time.timeScale = 1;
		Application.LoadLevel ("MainMenu");
	}
}

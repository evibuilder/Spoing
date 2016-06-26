using UnityEngine;
using System.Collections;
//using UnityEditor.SceneManagement;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RestartLevel(){
		Application.LoadLevel (Application.loadedLevel);
		//UnityEditor.SceneManagement.EditorSceneManager.LoadScene (UnityEditor.SceneManagement.EditorSceneManager.loadedSceneCount);
		//EditorSceneManager.LoadScene (EditorSceneManager.loadedSceneCount);
	}

	public void ExitLevel(){
		Application.LoadLevel ("MainMenu");
		//EditorSceneManager.LoadScene ("MainMenu");
	}
	public void Play(){
		Application.LoadLevel (1);
		//EditorSceneManager.LoadScene (1);
	}
	public void ExitGame(){
		Application.Quit ();
	}
}

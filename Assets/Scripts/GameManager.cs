using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		//UnityEditor.SceneManagement.EditorSceneManager.LoadScene (UnityEditor.SceneManagement.EditorSceneManager.loadedSceneCount);
		//EditorSceneManager.LoadScene (EditorSceneManager.loadedSceneCount);
	}

	public void ExitLevel(){
		SceneManager.LoadScene ("MainMenu");
	}
	public void Play(){
		SceneManager.LoadScene ("PlayGround2");
	}
	public void ExitGame(){
		Application.Quit ();
	}
}

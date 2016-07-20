using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private int currentScene;

	// Use this for initialization
	void Start () {
        currentScene = 0;
	}
	
	// Update is called once per frame
	void Update () {
        currentScene = SceneManager.GetActiveScene().buildIndex;
	}

	public void RestartLevel(){
        SceneManager.LoadScene(currentScene);
		//UnityEditor.SceneManagement.EditorSceneManager.LoadScene (UnityEditor.SceneManagement.EditorSceneManager.loadedSceneCount);
		//EditorSceneManager.LoadScene (EditorSceneManager.loadedSceneCount);
	}

	public void ExitLevel(){
        SceneManager.LoadScene (0);
	}
	public void Play(){
        SceneManager.LoadScene (1);
	}
    
    public void NextLevel()
    {
        if(currentScene != SceneManager.sceneCount - 1)
            SceneManager.LoadScene(currentScene + 1);
        
    }

	public void ExitGame(){
		Application.Quit ();
	}
}

using UnityEngine;
using System.Collections;
//using UnityEditor.SceneManagement;

public class LoadLevelUtil : MonoBehaviour {

	public void LoadLevel (int level){
		Application.LoadLevel(level);
		//EditorSceneManager.LoadScene (level);
	}
}

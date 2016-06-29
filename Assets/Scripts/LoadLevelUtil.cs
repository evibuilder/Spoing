using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevelUtil : MonoBehaviour {

	public void LoadLevel (int level){
		SceneManager.LoadScene (level);
	}
}

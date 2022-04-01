using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	//Loads of scene of a given name
	public void LoadLevel(string name) {
		SceneManager.LoadScene (name);
	}

	//Exits application
	public void QuitRequest() {
		Application.Quit();
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	// Defaults a music player instance to non-existant.
	static MusicPlayer instance = null;

	//Each time the state screen is awaken, check if a music player instance is alive.
	void Awake() {
		//If there is is an instance of musicplayer, then destroy the musicplayer trying to load.
		if (instance != null) {
			Destroy (gameObject);

		//If there is no music player running, set the player to an instance of itself, 
		//and dont destroy on load through different scenes.
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
		}
	}
}

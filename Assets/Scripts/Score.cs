using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	//Saves score within the class, not an object/instance of the class, so it is not destroyed.
	static int score = 0;
	Text scoreText;

	//On start, resets score to 0 and updates the text
	void Start() {
		score = 0;
		scoreText = GetComponent<Text> ();
		scoreText.text = score.ToString ();
	}

	//Adds the points scored onto the current score and updates the text
	public void ScorePoint (int points) {
		score += points;
		scoreText.text = score.ToString();
	}

	//Returns the stored score as an int
	public static int GetScore() {
		return score;
	}
}

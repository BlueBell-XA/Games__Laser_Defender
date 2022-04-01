using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour {

	Text finalScoreText;

	// Gets final score text element on awake and calls GetScore method from the Score class and converts the int to a string.
	void Awake() {
		finalScoreText = GetComponent<Text> ();
		finalScoreText.text = Score.GetScore ().ToString ();
	}
}

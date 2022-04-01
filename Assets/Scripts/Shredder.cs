using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour {

	//Destroys anything that contacts the shredder collider.
	//Keeps game clean and memory down.
	void OnTriggerEnter2D (Collider2D col) {
		Destroy (col.gameObject);
	}
}

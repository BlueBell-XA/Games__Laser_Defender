using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	//Called as an event after the explosion animation to remove from game.
	void DestroyGameObject() {
		Destroy (gameObject);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserSpeed : MonoBehaviour {

	public float enemyLaserSpeed = 10f;
	AudioSource audioSound;
	PlayerController player;
	float slowMoPitch;
	float originalPitch;

	//On awake, get the AudioSoure component and set the original and slow motion pitches
	void Awake() {
		audioSound = GetComponent<AudioSource> ();
		originalPitch = audioSound.pitch;
		player = GameObject.Find ("Player").GetComponent<PlayerController> ();
		slowMoPitch = audioSound.pitch * player.SlowMoSpeed();
	}

	//Moves the enemy laser when created, smoothly, for use with slow motion.
	void Update() {
		transform.position += new Vector3 (0f, -enemyLaserSpeed * Time.deltaTime);
		if (player.slowMoEnabled()) {
			audioSound.pitch = slowMoPitch;
		} else {
			audioSound.pitch = originalPitch;
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float enemyHealth = 200f;
	public float averageShotsPerSecond = 0.5f;
	public float fireRateIncreasePercent = 40f;
	public int scoreValue = 100;

	public GameObject enemyLaserPrefab;
	public AudioClip enemyDie;
	public GameObject explosion;

	EnemySpawner enemySpawner;
	float xMin;
	float xMax;
	float enemyGameUnitSize = 1;
	static float newFireRate;

	//On start, calculates min and max X movement based on camera viewport.
	//Finds the enemyspawner object.
	//Invokes methods with a delay.
	//Sets the defined fire rate to a new variable to reset the rate each time enemy spawns.
	void Start() {
		float zDistance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint (new Vector3(0,0,zDistance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint (new Vector3(1,0,zDistance));
		xMin = leftMost.x + (enemyGameUnitSize/2);
		xMax = rightMost.x - (enemyGameUnitSize/2);
		enemySpawner = GameObject.Find("EnemyFormation").GetComponent<EnemySpawner> ();
		Invoke ("RandomFire", Random.Range (1, 3));
		Invoke ("TriggerEnabled", 1);
		newFireRate = averageShotsPerSecond;
	}

	//Tells the enemySpawner (parent object) to move left or right when an enemy reaches the min or max postion.
	void Update() {
		if (transform.position.x <= xMin) {
			enemySpawner.MovingRight (true);
		} else if (transform.position.x >= xMax) {
			enemySpawner.MovingRight (false);
		}
	}

	//On collider collision;
	//Finds the projectile and score game object,
	//Calls the projectile Hit method to destroy the projectile,
	//Gets damage dealt by the player,
	//Then if the enemy health is 0, add points in the score class, and call EnemyDies method.
	void OnTriggerEnter2D (Collider2D col) {
		Projectile projectile = col.gameObject.GetComponent<Projectile>();
		Score scoreKeeper = GameObject.Find ("Score").GetComponent<Score> ();
		projectile.Hit ();
		enemyHealth -= projectile.GetDamage ("Player");
			if (enemyHealth <= 0) {
				scoreKeeper.ScorePoint (scoreValue);
				EnemyDies ();
			}
		}

	//Stops things colliding with enemy until called (1sec delay from Start).
	void TriggerEnabled() {
		gameObject.GetComponent<Collider2D> ().isTrigger = true;
	}

	//When called from OnTriggerEnter2D method, will play the explosion sound.
	//Enemy movement speed and fire rate is increased.
	//Explosion is created and the enemy is destroyed.
	void EnemyDies() {
		AudioSource.PlayClipAtPoint (enemyDie, transform.position);
		explosion = Instantiate (explosion);
		explosion.transform.position = transform.position;
		Destroy (gameObject);
		enemySpawner.speedIncrease ();
		newFireRate = newFireRate * ((100 + fireRateIncreasePercent) / 100);
	}

	//Calculates shots per second then creates the laser.
	//Repeats function over lifetime with randomised times between each shot.
	//Time range is between %50 and %150 of original time between shots.
	//All this happens when IsPlayerDead = false. This prevents errors by stopping the enemy shooting when player killed.
	void RandomFire() {
		if (!PlayerController.IsPlayerDead()) {
			float timeBetweenShots = 1 / newFireRate;
			float randomTime = Random.Range (timeBetweenShots*0.5f, timeBetweenShots*1.5f);
			Instantiate (enemyLaserPrefab, transform.position, Quaternion.AngleAxis(180, Vector3.forward));
			Invoke ("RandomFire", randomTime);
		}
	}
}
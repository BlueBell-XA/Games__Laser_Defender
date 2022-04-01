using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public float width = 7f;
	public float height = 3f;
	public float enemySpeed = 3f;
	public float spawnDelay = 0.3f;
	public float speedIncreasePercent = 10f;

	Transform[] enemyPositions;
	bool movingRight = false;
	int childCount;
	float newEnemySpeed;

	//On start;
	//Creates a new variable of enemy speed to mess with. Saves having another method to reset speed on scene load.
	//Collects the amount of enemy positions and creates an array of Transforms, with an array size equal to the number of enemy positions.
	//Loops through the array and stores each enemy position's transform in their own index.
	//Finally, calls the SpawnEnemy method as a coroutine to spawn the enemies.
	void Start() {
		newEnemySpeed = enemySpeed;
		childCount = transform.childCount;
		enemyPositions = new Transform[childCount];
		for (int p = 0; p < childCount; p++) {
			enemyPositions [p] = this.gameObject.transform.GetChild(p);
		}
		StartCoroutine ("SpawnEnemy");
	}

	//Method draws a cube gizmo within editor to easily see position & size of enemy spawner.
	public void OnDrawGizmos() {
		Gizmos.DrawWireCube (transform.position, new Vector3 (width, height));
	}

	//Move the enemy spawner position each frame, either left or right.
	//Direction depends on movingRight boolean, set from the enemy class.
	//If all enemy die, begin the coroutine of SpawnEnemy.
	void Update() {
		if (movingRight) {
			transform.position += new Vector3 (newEnemySpeed * Time.deltaTime, 0f);
		} else {
			transform.position += new Vector3 (-newEnemySpeed * Time.deltaTime, 0f);
		}

		if (AllEnemyDead()) {
			StartCoroutine ("SpawnEnemy");
		}
	}

	//When called, loop through each child (enemy positions).
	//Returns false when at least 1 enemy is found alive.
	//Returns true when all enemy are dead and resets the spawner movement speed.
	bool AllEnemyDead() {
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) {
				return false;
			}
		}
		newEnemySpeed = enemySpeed;
		return true;
	}

	//Called from the coroutines;
	//Loop through the enemyPosition array and spawns an enemy in each position.
	//NOTE:
	//An array of positions was also used to prevent enemies continually spawning if the player killed them quick enough.
	//A coroutine was used to use the YIELD constructor... this was the only way to implement spawn delay after implementing a position array.
	IEnumerator SpawnEnemy() {
		for (int p = 0; p < childCount; p++) {
			Instantiate (enemyPrefab, enemyPositions[p]);
			yield return new WaitForSeconds (spawnDelay);
		}
	}

	//When called (from the Enemy class), changes whether or not the spawner should be moving left or right,
	//depending on what min/max edge an enemy has hit.
	public void MovingRight (bool condition) {
		movingRight = condition;
	}

	//When called (from the Enemy class), loops through each enemy position in the spawner.
	//For each enemy that doesn't exist, increase the movement speed by speedIncreasePercent.
	public void speedIncrease() {
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0) {
				newEnemySpeed = newEnemySpeed * ((100 + speedIncreasePercent) / 100);
			}
		}
	}
}
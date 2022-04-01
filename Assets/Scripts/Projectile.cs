using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float playerDamage = 100f;
	public float enemyDamage = 100f;

	//Destroys the projectile when called
	public void Hit() {
		Destroy (gameObject);
	}

	//When called, returns the damage dealt of the type requested.
	//Returns 0 damage and an error if incorrect type was asked for.
	public float GetDamage (string damageType) {
		if (damageType == "Player") {
			return playerDamage;
		} else if (damageType == "Enemy") {
			return enemyDamage;
		} else {
			Debug.LogError ("Damage type doesn't exist. Returning 0 damage on collision. Check PlayerController or Enemy script.");
			return 0f;
		}
	}
}

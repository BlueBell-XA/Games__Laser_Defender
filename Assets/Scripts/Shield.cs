using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	//On collision with shield, destroy the object, destroy the shield, and call player.ShieldHit method.
	void OnTriggerEnter2D (Collider2D col) {
		Projectile projectile = col.gameObject.GetComponent<Projectile>();
		PlayerController player = GameObject.Find ("Player").GetComponent<PlayerController> ();
		projectile.Hit ();
		player.ShieldHit ();
		Destroy (gameObject);
	}
}
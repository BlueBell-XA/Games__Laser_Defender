using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour {

	// Draws gizmos aroudn the origin position to visualise location of empty object.
	void OnDrawGizmos() {
		Gizmos.DrawWireSphere (transform.position, 0.4f);
	}
}

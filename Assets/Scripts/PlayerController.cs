using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float fireRate = 4f;
	public float playerMoveSpeed = 10f;
	public float playerHealth = 100f;
	public float slowMotionGameSpeed = 0.4f;
	public float slowMotionUse = 3f;
	public float slowMotionRecharge = 0.5f;

	public GameObject playerLaserPrefab;
	public GameObject explosion;
	public GameObject shieldHitSound;
	public AudioClip die;
	public AudioClip startSlowMo;
	public AudioClip endSlowMo;

	AudioSource shieldAudio;
	Slider slowMotionSlider;
	float xMin;
	float xMax;
	float padding = 0.5f;
	float time = 0f;
	bool isSlowMo = false;
	bool sKeyEnabled;
	static bool playerDead;

	//On start, collect the min and max X movement values based on camera viewport.
	//Buffer/padding is added to stop player unit at the edge of the sprite.
	//Slow motion is turned off at the start, and playerDead boolean set to alive/false.
	void Start() {
		float zDistance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint (new Vector3(0,0,zDistance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint (new Vector3(1,0,zDistance));
		slowMotionSlider = GameObject.Find ("Slider").GetComponent<Slider> ();
		xMin = leftMost.x + padding;
		xMax = rightMost.x - padding;
		isSlowMo = false;
		playerDead = false;
		sKeyEnabled = true;
	}

	//Calls internal functions. See specific functions for more detail
	//Sets amount of slow motion after continually checking if slow motion is enabled. 
	//Continually checking avoids bugs with staying in slow-mo after restarting the game.
	void Update() {
		PlayerControls ();
		SlowMotionSliderValue ();
		ShieldKilledSoundPitch ();
		if (isSlowMo) {
			Time.timeScale = slowMotionGameSpeed;
		} else {
			Time.timeScale = 1f;
		}
	}

	//On collision with player;
	//Find the projectile that collided with the player,
	//Subtract the damage dealt by the enemy to the player from health,
	//Call projectile Hit function to destroy the projectile
	//If player health is 0, call PlayerDeath function.
	void OnTriggerEnter2D (Collider2D col) {
		Projectile projectile = col.gameObject.GetComponent<Projectile> ();
		playerHealth -= projectile.GetDamage ("Player");
		projectile.Hit ();
		if (playerHealth <= 0) {
			PlayerDeath ();
		}
	}

	//Moves the player left or right, instantly, by defined player move speed. 
	//Speed multiplied with time.deltaTime to smooth out the movement, based on framerate.
	//Slow down timeby pressing S key. 1 is original game speed, lower values are slow motion.
	//Player position is clamped between min and max X values and player position updated after movement.
	//NOTE: when current player position reaches the max or min values, the clamp returns those values
	//Which is used to update the player position, thus, clamping it.
	void PlayerControls() {
		//Moves the player left or right, with left being the override.
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.position += new Vector3 (-playerMoveSpeed * Time.deltaTime, 0f);
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			transform.position += new Vector3 (playerMoveSpeed * Time.deltaTime, 0f);
		}
		//Calls Fire / Shoot method.
		if (Input.GetKey (KeyCode.Space)) {
			Fire ();
		}
		//Starts slow motion effect if key is enabled.
		if (Input.GetKeyDown (KeyCode.S) && sKeyEnabled) {
			AudioSource.PlayClipAtPoint (startSlowMo, transform.position);
			if (Input.GetKey (KeyCode.S) && sKeyEnabled) {
				isSlowMo = true;
			}
		}
		//Disables 'S' key and removes all slow motion effects when the slider reaches 0. 
		//Unable to press 'S' key again for 0.3sec.
		if (slowMotionSlider.value <= 0) {
			AudioSource.PlayClipAtPoint (endSlowMo, transform.position);
			isSlowMo = false;
			sKeyEnabled = false;
			Invoke ("EnableSKey", 0.3f);
		}
		//If 'S' key released and slow motion was still active, will end all slow motion effects.
		if (Input.GetKeyUp(KeyCode.S) && isSlowMo) {
			AudioSource.PlayClipAtPoint (endSlowMo, transform.position);
			isSlowMo = false;
		}
		//Clamps the x position to precent moving out of the camera area.
		float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}

	//Creates a player laser at current position. Velocity controlled by each laser.
	//Firerate is restricted by comparing to Time.time to avoid bugs with enemy difficulty, particularly in slow motion.
	//After each shot, time float is updated to use and compare for the next iteration.
	void Fire() {
		if (time+(1/fireRate) <= Time.time) {
			Instantiate (playerLaserPrefab, transform.position, Quaternion.identity);
			time = Time.time;
		}
	}

	//Called during Update;
	//If slow motion is active and there is a shieldHitSound gameObject in existance, then slow down the pitch of the audiosource.
	//If the slow motion not active, play, or continue playing, the audio source at normal pitch.
	void ShieldKilledSoundPitch() {
		if (isSlowMo && GameObject.Find ("Shield Hit Sound(Clone)") != null) {
			shieldAudio.pitch = slowMotionGameSpeed;
		} else if (!isSlowMo && GameObject.Find ("Shield Hit Sound(Clone)") != null) {
			shieldAudio.pitch = 1f;
		}
	}

	//On method call, player object is disabled, and explosion and sound is created.
	//NOTE: Invoke will not call function if the attached object is destroyed before or while invoking delay, thus setting to inactive.
	void PlayerDeath() {
		AudioSource.PlayClipAtPoint (die, transform.position);
		Instantiate (explosion, transform.position, Quaternion.identity);
		Invoke ("GameOver", 0.7f);
		playerDead = true;
		gameObject.SetActive (false);
	}

	//Loads the Game Over scene when called. Method is seperate to allow for time delay.
	void GameOver() {
		SceneManager.LoadScene ("Game Over");
	}

	//Returns the current slow motion status when called
	public bool slowMoEnabled() {
		return isSlowMo;
	}

	//Returns the amount of slow motion when called.
	//Set like this to avoid changing the slow motion in multiple places.
	public float SlowMoSpeed() {
		return slowMotionGameSpeed;
	}

	//When called from the Shield class, will create a shield hit sound in the form of a gameObject.
	//The audiosource of the gameObject is stored and then the object destroyed after 2 seconds.
	public void ShieldHit() {
		Instantiate (shieldHitSound, transform.position, Quaternion.identity);
		shieldAudio = GameObject.Find("Shield Hit Sound(Clone)").GetComponent<AudioSource> ();
		Destroy (GameObject.Find("Shield Hit Sound(Clone)"), 2f);
	}

	//Returns playerDead status. 
	//Called from Enemy class and used to stop enemy firing when dead to prevent errors.
	static public bool IsPlayerDead() {
	return playerDead;
	}

	//Called in Update, uses and recharges the slow motion bar depending if slow motion is activated or not.
	void SlowMotionSliderValue() {
		if (isSlowMo) {
			slowMotionSlider.value -= slowMotionUse * Time.deltaTime;
		} else {
			slowMotionSlider.value += slowMotionRecharge * Time.deltaTime;
		}
	}

	//When called, enables S key. Used to enable after invoke time delay.
	void EnableSKey() {
		sKeyEnabled = true;
	}
}
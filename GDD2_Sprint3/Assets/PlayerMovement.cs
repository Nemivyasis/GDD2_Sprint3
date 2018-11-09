/** Jonathan So, jds7523@rit.edu
 * Handles player movement, including controls for setting the gravity of the player.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public KeyCode tiltUp, tiltDown, tiltLeft, tiltRight; // Keyboard controls for triggering the gravity changes.

	private Joycon j;
	private Vector3 accel; // Acceleration vector of the controller.

	private enum GRAVITY {UP, DOWN, LEFT, RIGHT};
	private GRAVITY currGrav; // The current state of gravity.

	private Vector2 GRAV_DOWN = new Vector3(0.0f, -2.5f);
	private Vector2 GRAV_UP = new Vector3(0.0f, 2.5f);
	private Vector2 GRAV_LEFT = new Vector3(-2.5f, 0.0f);
	private Vector2 GRAV_RIGHT = new Vector3(2.5f, 0.0f);

	// Set the initial gravity vector to point down.
	private void Awake() {
		Physics2D.gravity = GRAV_DOWN;
		currGrav = GRAVITY.DOWN; // Start with gravity pointing down.
	}

	// Get the Joycon object; note that the JoyconManager is required.
	private void Start() {
		// Get the public Joycon object attached to the JoyconManager in scene
		j = JoyconManager.Instance.j;
	}

	/** Checks the controller for its acceleration and switches gravity based on the controller.
	 * Also makes sure we don't call a Gravity Switch when it's not necessary (e.g calling a GravitySwitch(DOWN) when we're already pointing down).
	 * PRECONDITION: There must be a Joycon present to call this code.
	 */
	private void GravityCheck() {
		if (currGrav != GRAVITY.DOWN && accel.y + (Mathf.Abs(accel.z)) >= 1.0f) { // Our controller is upright. Gravity points down.
			GravitySwitch(GRAVITY.DOWN);
		} else if (currGrav != GRAVITY.UP && accel.y + (-Mathf.Abs(accel.z)) <= -1.0f) { // Our controller is upside-down. Gravity points up.
			GravitySwitch(GRAVITY.UP);
		} else if (currGrav != GRAVITY.RIGHT && accel.x + (Mathf.Abs(accel.z)) >= 1.0f) { // Our controller is 90 degrees clockwise. Gravity points right.
			GravitySwitch(GRAVITY.RIGHT);
		} else if (currGrav != GRAVITY.LEFT && accel.x + (-Mathf.Abs(accel.z)) <= -1.0f) { // Our controller is 90 degrees counterclockwise. Gravity points left.
			GravitySwitch(GRAVITY.LEFT);
		} 
	}

	/** Switches gravity and rumbles the controller.
	 * param[newGrav] - a GRAVITY enum for the four possible directions of gravity.
	 */
	private void GravitySwitch(GRAVITY newGrav) {
		if (newGrav.Equals(GRAVITY.DOWN)) {
			Physics2D.gravity = GRAV_DOWN;
			currGrav = GRAVITY.DOWN;
		} else if (newGrav.Equals(GRAVITY.UP)) {
			Physics2D.gravity = GRAV_UP;
			currGrav = GRAVITY.UP;
		} else if (newGrav.Equals(GRAVITY.RIGHT)) {
			Physics2D.gravity = GRAV_RIGHT;
			currGrav = GRAVITY.RIGHT;
		} else if (newGrav.Equals(GRAVITY.LEFT)) {
			Physics2D.gravity = GRAV_LEFT;
			currGrav = GRAVITY.LEFT;
		}
		// Rumble the controller for feedback.
		if (j != null) {
			j.SetRumble (160, 320, 0.6f, 200);
		}
	}

	/** Handles both controller and keyboard inputs for gravity switching.
	 * Checks our current gravity against the player's desired gravity, and, 
	 * if we want to switch gravity, then call the code to do so.
	 */
	void Update () {
		if (j != null) { 
			accel = j.GetAccel(); 
			GravityCheck();
		}

		// Handle keyboard inputs for switching the gravity of the player.
		if (Input.GetKeyDown(tiltUp) && currGrav != GRAVITY.UP) { // Set Gravity to Up.
			GravitySwitch(GRAVITY.UP);
		} else if (Input.GetKeyDown(tiltDown) && currGrav != GRAVITY.DOWN) { // Set Gravity to Down.
			GravitySwitch(GRAVITY.DOWN);
		} else if (Input.GetKeyDown(tiltLeft) && currGrav != GRAVITY.LEFT) { // Set Gravity to Left.
			GravitySwitch(GRAVITY.LEFT);
		} else if (Input.GetKeyDown(tiltRight) && currGrav != GRAVITY.RIGHT) { // Set Gravity to Right.
			GravitySwitch(GRAVITY.RIGHT);
		}


	}
}

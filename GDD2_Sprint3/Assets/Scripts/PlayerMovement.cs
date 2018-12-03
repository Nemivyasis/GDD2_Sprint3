/** Jonathan So, jds7523@rit.edu
 * Noah Smith, nes9968@rit.edu
 * Handles player movement, including controls for setting the gravity of the player.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public bool rotLevel = false;
	public KeyCode tiltUp, tiltDown, tiltLeft, tiltRight; // Keyboard controls for triggering the gravity changes.

	private Joycon j;
	private Vector3 accel; // Acceleration vector of the controller.

	private enum GRAVITY {UP, DOWN, LEFT, RIGHT};
	private GRAVITY currGrav; // The current state of gravity.

	private Vector2 GRAV_DOWN = new Vector3(0.0f, -2.5f);
	private Vector2 GRAV_UP = new Vector3(0.0f, 2.5f);
	private Vector2 GRAV_LEFT = new Vector3(-2.5f, 0.0f);
	private Vector2 GRAV_RIGHT = new Vector3(2.5f, 0.0f);

    private enum MOVE { LEFT, RIGHT };
    private MOVE currentMovement;
    public Vector3 currentPos;
    float[] jStick; // store the value of joycon stick position

    public float maxFallSpeed;

	public AudioClip gravSFX, hitFloorSFX;
	private AudioSource audi; // For playing all of our sound effects.

    private Animator anim;

    ResetDeathManager deathCheck;

    //rotation speed of the character
    float rotationSpeed;

    private LevelRotScript levelRot;

    private Rigidbody2D rb;

    // Set the initial gravity vector to point down.
    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        levelRot = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelRotScript>();

		Physics2D.gravity = GRAV_DOWN;
		currGrav = GRAVITY.DOWN; // Start with gravity pointing down.

        deathCheck = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ResetDeathManager>();
		audi = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

	// Get the Joycon object; note that the JoyconManager is required.
	private void Start() {
		// Get the public Joycon object attached to the JoyconManager in scene
		j = JoyconManager.Instance.j;

        rotationSpeed = 100.0f;
        currentPos = transform.position;
    }

	/** Checks the controller for its acceleration and switches gravity based on the controller.
	 * Also makes sure we don't call a Gravity Switch when it's not necessary (e.g calling a GravitySwitch(DOWN) when we're already pointing down).
	 * PRECONDITION: There must be a Joycon present to call this code.
	 */
	private void GravityCheck() {
		if (!(j.GetButton(Joycon.Button.SL) || j.GetButton(Joycon.Button.SR))) {
			return;
		}
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
            if (!rotLevel)
            {
                Physics2D.gravity = GRAV_DOWN;
                rb.velocity = new Vector2(0, -rb.velocity.magnitude);
            }
                currGrav = GRAVITY.DOWN;

		} else if (newGrav.Equals(GRAVITY.UP)) {
            if (!rotLevel)
            {
                Physics2D.gravity = GRAV_UP;
                rb.velocity = new Vector2(0, rb.velocity.magnitude);
            }
            currGrav = GRAVITY.UP;

        } else if (newGrav.Equals(GRAVITY.RIGHT)) {
            if (!rotLevel)
            {
                Physics2D.gravity = GRAV_RIGHT;
                rb.velocity = new Vector2( rb.velocity.magnitude, 0);
            }
                currGrav = GRAVITY.RIGHT;

        } else if (newGrav.Equals(GRAVITY.LEFT)) {
            if (!rotLevel)
            {
                Physics2D.gravity = GRAV_LEFT;
                rb.velocity = new Vector2(-rb.velocity.magnitude, 0);
            }
                currGrav = GRAVITY.LEFT;
		}
		// Play a sound effect for feedback.
		audi.PlayOneShot(gravSFX);
		// Rumble the controller for feedback.
		if (j != null) {
			//j.SetRumble (160, 320, 0.6f, 200);
		}
	}

    /* Changes position of the player
     * Changes position.x or position.y relative to the direction of gravity
     */
    private void Movement(MOVE tempMove)
    {

        if (!rotLevel)
        {

            if (currGrav.Equals(GRAVITY.UP) || currGrav.Equals(GRAVITY.DOWN))
            {
                if (tempMove.Equals(MOVE.LEFT))
                {
                    currentPos.x += -0.05f;
                    currentMovement = MOVE.LEFT;
                    transform.position = currentPos;
                }
                else if (tempMove.Equals(MOVE.RIGHT))
                {
                    currentPos.x += 0.05f;
                    currentMovement = MOVE.RIGHT;
                    transform.position = currentPos;
                }
            }
            else if (currGrav.Equals(GRAVITY.LEFT) || currGrav.Equals(GRAVITY.RIGHT))
            {
                if (tempMove.Equals(MOVE.LEFT))
                {
                    currentPos.y += -0.05f;
                    currentMovement = MOVE.LEFT;
                    transform.position = currentPos;
                }
                else if (tempMove.Equals(MOVE.RIGHT))
                {
                    currentPos.y += 0.05f;
                    currentMovement = MOVE.RIGHT;
                    transform.position = currentPos;
                }
            }
        }
        else
        {
            if (tempMove.Equals(MOVE.LEFT))
            {
                currentPos.x += -0.05f;
                currentMovement = MOVE.LEFT;
                transform.position = currentPos;
            }
            else if (tempMove.Equals(MOVE.RIGHT))
            {
                currentPos.x += 0.05f;
                currentMovement = MOVE.RIGHT;
                transform.position = currentPos;
            }
        }
    }

	/** Handles both controller and keyboard inputs for gravity switching.
	 * Checks our current gravity against the player's desired gravity, and, 
	 * if we want to switch gravity, then call the code to do so.
	 */
	void Update () {

        //don't move if dead
        if (deathCheck.isDead)
        {
            return;
        }
        
        if(rb.velocity.magnitude > maxFallSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxFallSpeed);
        }

        if (j != null) { 
			accel = j.GetAccel(); 
			GravityCheck();
		}
        
		// Handle keyboard inputs for switching the gravity of the player.
		if (Input.GetKeyDown(tiltUp) && currGrav != GRAVITY.UP) { // Set Gravity to Up.
			GravitySwitch(GRAVITY.UP);
        } else if (Input.GetKeyDown(tiltDown) && currGrav != GRAVITY.DOWN) { // Set Gravity to Down.
			GravitySwitch(GRAVITY.DOWN);
            //transform.Rotate(new Vector3(0, 0, 1) * rotationSpeed * Time.deltaTime, Space.World);
        } else if (Input.GetKeyDown(tiltLeft) && currGrav != GRAVITY.LEFT) { // Set Gravity to Left.
			GravitySwitch(GRAVITY.LEFT);
		} else if (Input.GetKeyDown(tiltRight) && currGrav != GRAVITY.RIGHT) { // Set Gravity to Right.
			GravitySwitch(GRAVITY.RIGHT);
		}

        if (!rotLevel)
        {
            //handles the rotation of the character when the y gravity is manipulated
            if (currGrav == GRAVITY.UP && (Vector3.Distance(transform.eulerAngles, new Vector3(0.0f, 0.0f, 180.0f)) > 0.01f))
            {
                transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0.0f, 0.0f, 180.0f), Time.deltaTime * rotationSpeed);
            }
            else if (currGrav == GRAVITY.DOWN && (Vector3.Distance(transform.eulerAngles, Vector3.zero) > 0.01f))
            {
                transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, Vector3.zero, Time.deltaTime * rotationSpeed);
            }
            else if (currGrav == GRAVITY.RIGHT && (Vector3.Distance(transform.eulerAngles, new Vector3(0.0f, 0.0f, 90.0f)) > 0.01f))
            {
                transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0.0f, 0.0f, 90.0f), Time.deltaTime * rotationSpeed);
            }
            else if (currGrav == GRAVITY.LEFT && (Vector3.Distance(transform.eulerAngles, new Vector3(0.0f, 0.0f, -90.0f)) > 0.01f))
            {
                transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, new Vector3(0.0f, 0.0f, -90.0f), Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            levelRot.Rotate((int)currGrav);
        }
        
        currentPos = transform.position;
        // Keyboard inputs for player movement
        if (Input.GetKey(KeyCode.A))
        {
            Movement(MOVE.LEFT);
            anim.SetBool("IsFloating", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Movement(MOVE.RIGHT);
            anim.SetBool("IsFloating", true);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        } else
        {
            anim.SetBool("IsFloating", false);
        }


        // switch stick controls
        if (j != null)
        {
            jStick = j.GetStick(); // jStick[0] is x position of stick, jStick[1] is y position
            if (currGrav.Equals(GRAVITY.DOWN))
            {
                if (jStick[1] > 0.0f) // joystick is to the left
                {
                    Movement(MOVE.LEFT);
                }
                else if (jStick[1] < 0.0f) // joystick is to the right
                {
                    Movement(MOVE.RIGHT);
                }
            }
            else if (currGrav.Equals(GRAVITY.UP))
            {
                if (jStick[1] < 0.0f) // joystick is to the left
                {
                    Movement(MOVE.LEFT);
                }
                else if (jStick[1] > 0.0f) // joystick is to the right
                {
                    Movement(MOVE.RIGHT);
                }
            }
            else if (currGrav.Equals(GRAVITY.RIGHT))
            {
                if (jStick[0] < 0.0f) // joystick is to the left
                {
                    Movement(MOVE.LEFT);
                }
                else if (jStick[0] > 0.0f) // joystick is to the right
                {
                    Movement(MOVE.RIGHT);
                }
            }
            else if (currGrav.Equals(GRAVITY.LEFT))
            {
                if (jStick[0] > 0.0f) // joystick is to the left
                {
                    Movement(MOVE.LEFT);
                }
                else if (jStick[0] < 0.0f) // joystick is to the right
                {
                    Movement(MOVE.RIGHT);
                }
            }

        }
    }

	// Code to play the sound for hitting the floor.
	private void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag.Equals("Platform")) {
			audi.PlayOneShot(hitFloorSFX);
		}
	}

}

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (SpriteRenderer))]
public class Player : MonoBehaviour {

	//Editor Customizable
	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	public float moveSpeed = 6;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;
	public string player = "P1";  	//This is for Multiplayer Support
	public float health = 100;
	public bool dead = false;

	float facing = 1;
	float gravity;
	float jumpVelocity;
	public Vector3 velocity;
	float velocityXSmoothing;

	//Class References
	SpriteRenderer character;
	Controller2D controller;
	//ItemEnity item;
	//Class Reference to Item Entity Here!

	void Start () {
		character = GetComponent<SpriteRenderer> ();
		controller = GetComponent<Controller2D> ();
		controller.CatchPlayer (this);
		UpdateGravity ();
	}

	void Update(){
	
		UpdateGravity ();
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal_" + player), Input.GetAxisRaw ("Vertical_" + player));

		//This is a fix for velocity accumulation while on the ground
		if (controller.collisions.above || controller.collisions.below) { 		
			velocity.y = 0;
		}

		if (!IsDead ()) {
			


			//Sprite Direction
			if (input.x != 0) { 		
				facing = Mathf.Sign (input.x);
				character.flipX = (facing == -1);
			}

			//Jump
			if (Input.GetButtonDown ("Jump_" + player) && controller.collisions.below) {
				velocity.y = jumpVelocity;
			}

			//Hit/Fire Weapon
			if (Input.GetButtonDown ("Fire_" + player)) {
				//Add Weapon Fire Support Here
				controller.Punch (facing);
			}

			//Horizontal Velocity Smoothing
			float targetVelocityX = input.x * moveSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing,
				(controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		} else if (!dead) { //Fix!
			velocity.x = 0;
			Death ();
		} else {
			velocity.x = Mathf.SmoothDamp (velocity.x, 0, ref velocityXSmoothing, 0.05f);
		}

		//Gravity and Move Player for Input
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}

	bool IsDead(){
		return health <= 0;
	}

	//Drop Item, Animate Death, Ect
	public void Death(){
		dead = true;
	}

	//This Method is for Converting the More Intuitive Jump Height and Time
	//Variables into Gravity and Jump Velocity to Use on Character
	//Calculations Based on Physics
	public void UpdateGravity(){
		gravity = -(jumpHeight * 2) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs (gravity) * timeToJumpApex;
	}
}

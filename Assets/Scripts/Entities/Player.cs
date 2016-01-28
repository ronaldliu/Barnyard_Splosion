using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
//[RequireComponent (typeof (SkeletonAnimation))]
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
	SkeletonAnimation anim;
	MeshRenderer character;
	Controller2D controller;
	//ItemEnity item;
	//Class Reference to Item Entity Here!

	void Start () {
		character = GetComponent<MeshRenderer> ();
		controller = GetComponent<Controller2D> ();
		anim = GetComponent<SkeletonAnimation> ();

		controller.CatchPlayer (this);
		UpdateGravity ();
	}

	void Update(){

		UpdateGravity ();
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal_" + player), Input.GetAxisRaw ("Vertical_" + player));

		//This is a fix for velocity accumulation while on the ground
		if (controller.collisions.above || controller.collisions.below) { 		
			velocity.y = 0;
			print("yes");
			print(velocity.y);

		}

		if (!IsDead ()) {

			//Sprite Direction
			if (input.x != 0) { 		
				facing = Mathf.Sign (input.x);
				character.transform.localScale = new Vector3(facing *1,1,1);
			}

			//Jump
			if (Input.GetButtonDown ("Jump_" + player) && controller.collisions.below) {
				
				velocity.y = jumpVelocity;
				print(velocity.y);

			}

			//Hit/Fire Weapon
			if (Input.GetButtonDown ("Fire_" + player)) {
				//Add Weapon Fire Support Here
				anim.state.SetAnimation (2, "Poke", false);
				controller.Punch (facing);
			}

			//Horizontal Velocity Smoothing

			float targetVelocityX = input.x * moveSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing,
				(controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			print(velocity.y);

			if (targetVelocityX != 0) {
				//anim.state.SetAnimation (1, "animation", true);
			} else {
				//anim.state.SetAnimation (1, "Standing", true);

				//anim.state.ClearTrack(1);

			}
		
		} else if (!dead) { //Fix!
			velocity.x = 0;
			Death ();
		} else {
			velocity.x = Mathf.SmoothDamp (velocity.x, 0, ref velocityXSmoothing, 0.05f);
		}
		print(velocity.y);

		//Gravity and Move Player for Input
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
		print(velocity.y);

	}

	bool IsDead(){
		
		return health <= 0;
	}

	//Drop Item, Animate Death, Ect
	public void Death(){
		print (player);
	//anim.state.SetAnimation (20, "death", true);
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

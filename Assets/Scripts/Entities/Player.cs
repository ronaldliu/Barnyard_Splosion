using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
//[RequireComponent (typeof (SkeletonAnimation))]
public class Player : MonoBehaviour {

    //this is a testing variable
    private bool oneShot = true;

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
	[HideInInspector]
	public BoxCollider2D boxCollider;

	//Class References
	SkeletonAnimation anim;
	MeshRenderer character;
	Controller2D controller;
	//ItemEnity item;
	//Class Reference to Item Entity Here!

	void Start () {
		boxCollider = GetComponent<BoxCollider2D> ();
		character = GetComponent<MeshRenderer> ();
		controller = GetComponent<Controller2D> ();
		anim = GetComponent<SkeletonAnimation> ();
		anim.state.ClearTrack(1);
		controller.CatchPlayer (this);
		UpdateGravity ();
	}

	void Update(){

		UpdateGravity ();
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal_" + player), Input.GetAxisRaw ("Vertical_" + player));
		Vector2 aimer = new Vector2 (Input.GetAxisRaw ("AimH_" + player), Input.GetAxisRaw ("AimV_" + player));

		//This is a fix for velocity accumulation while on the ground
		if (controller.collisions.above || controller.collisions.below) { 		
			velocity.y = 0;

		}

		if (!IsDead ()) {

			if (!aimer.Equals (Vector2.zero)) {
				print ("Hi");

			}
			//Sprite Direction
			if(input.x != 0) { 		
				facing = Mathf.Sign (input.x);
				character.transform.localScale = new Vector3(facing *.05f,.05f,1);
			}

			//Jump
			if (Input.GetButtonDown ("Jump_" + player) && controller.collisions.below) {
				
				velocity.y = jumpVelocity;
			}

            if(velocity.y != 0)
            {
                anim.state.SetAnimation(2, "Jump", false);
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

			if (targetVelocityX != 0) {
                if (oneShot)
                {
                    anim.state.SetAnimation(1, "animation", true);
                    oneShot = false;
                }
				   

			} else {
				anim.state.SetAnimation (1, "Standing", true);
                oneShot = true;
				anim.state.ClearTrack(1);

			}
		
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

	public bool IsDead(){
		
		return health <= 0;
	}

	//Drop Item, Animate Death, Ect
	public void Death(){
		print (player);
		anim.state.ClearTracks ();
		anim.state.SetAnimation (1, "Death", false);
		boxCollider.size = new Vector2 (55, 22);
		boxCollider.offset = new Vector2 (-6, -16.4f);
		gameObject.layer = 14;
		controller.collisionMask =  1 << LayerMask.NameToLayer("Obsticle");
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

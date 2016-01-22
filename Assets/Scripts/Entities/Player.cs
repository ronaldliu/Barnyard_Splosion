using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (SpriteRenderer))]
public class Player : MonoBehaviour {
	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	public float moveSpeed = 6;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;


	
	float facing = 1;
	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	SpriteRenderer character;
	Controller2D controller;

	void Start () {
		character = GetComponent<SpriteRenderer> ();
		controller = GetComponent<Controller2D> ();
		UpdateGravity ();
	}
	void Update(){
		UpdateGravity ();
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		float targetVelocityX = input.x * moveSpeed;

		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);

		if(input.x != 0){
			facing = Mathf.Sign (input.x);
		}
		character.flipX = facing == -1;

		if ((Input.GetButtonDown ("Jump") || input.y > .35f) && controller.collisions.below) {
			print (jumpVelocity);
			velocity.y = jumpVelocity;
		}

		if (Input.GetButtonDown ("Fire1")) {
			controller.Punch (facing);
			print ("Fire");
		}
	}
	public void UpdateGravity(){
		gravity = -(jumpHeight * 2) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs (gravity) * timeToJumpApex;
		//print("Gravity: " + gravity + " Jump Vel: " +jumpVelocity);

	}
}

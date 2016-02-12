
using UnityEngine;
using System.Collections;

public class Controller2D : RaycastController{

	//Editor Customizable
	public float punchLength = 5;
	public LayerMask fightingMask;	 //Used to Hit Other Players
	public CollisionInfo collisions; //Collision Struct
	public LayerMask pickUpMask;
	public float punchForce = 20;

	//For Slope Handling
	float maxClimbAngle = 80;
	float maxDescendAngle = 75;

	Player me; //Reference to Player Under Control

	public override void Start(){
		base.Start ();
	}

	public void CatchPlayer(Player me){
		this.me = me;
	}
	//Moves Character With Collision Accounted for.
	//USE THIS NOT TRANSFORM.TRANSLATE!
	public void Move(Vector3 velocity, bool standingonPlatform = false){

		UpdateRaycastOrigins ();
		collisions.Reset ();
		collisions.velocityOld = velocity;

		//Going Down Slopes
		if (velocity.y < 0) {
			DescendSlope (ref velocity);
		}

		//Only Collide Horizontally if Moving So
		if (velocity.x != 0) {
			HorizonatalCollisions (ref velocity);
		}

		//Only Collide Vertically if Moving So
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}

		//After Collision Detection Then Translate!
		transform.Translate (velocity);

		if (standingonPlatform) {
			collisions.below = true;
		}
	}

	void HorizonatalCollisions(ref Vector3 velocity){
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x)+ skinWidth;

		//Loop For cycling through Cast Rays Up the Side of the Character
		for (int i = 0; i <  horizontalRayCount; i++) {
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);

			//Raycast Detection of Collisions with only collisionMask Layer being considered
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask + fightingMask); 

			Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red); //Draw Red Lines in Scene for Debuging Purposes

			if (hit) { //Case Ray Hits Considered Target
				int collisionLayer = hit.transform.gameObject.layer;
				if(hit.distance == 0 && collisionLayer == LayerMask.NameToLayer ("Obsticle")){
					me.velocity.x = 10 * me.facing;
				}
				if (hit.distance == 0 || (LayerMask.NameToLayer("Platforms") == collisionLayer)) { //if inside of object allow player to move freely
					continue;
				}

				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);

				if (i == 0 && slopeAngle <= maxClimbAngle){//&& slopeAngle > 0) { //Slope Handling

					if (collisions.descendingSlope) {
						collisions.descendingSlope = false;
						velocity = collisions.velocityOld;
					}

					float distanceToSlopeStart = 0;

					if (slopeAngle != collisions.slopeAngleOld) {
						distanceToSlopeStart = hit.distance - skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}

					ClimbSlope (ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}

				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {

					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					if(collisions.climbingSlope){
						velocity.y = Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x);
					}

					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
				}
			}
		}
	}

	//Vertical Collisions Are Very Similar to Horizontal
	void VerticalCollisions(ref Vector3 velocity){
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y)+ skinWidth;
	
		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask + fightingMask);
				
			Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if (hit) {
				int collisionLayer = hit.transform.gameObject.layer;
				if(hit.distance == 0 && collisionLayer == LayerMask.NameToLayer ("Obsticle")){
					me.velocity.x = 10 * me.facing;
				}
				if ((hit.distance == 0 && (fightingMask.value & 1 << collisionLayer) == 0)){ //if inside of object allow player to move freely
					continue;
				}
				if (LayerMask.NameToLayer ("Platforms") == collisionLayer) {
					collisions.droppable = true;
				} else {
					collisions.droppable = false;

				}
				if (directionY > 0 && LayerMask.NameToLayer ("Platforms") == collisionLayer) {
					continue;
				}

				if (collisionLayer == LayerMask.NameToLayer ("Platforms") || directionY == -1) {
					velocity.y = (hit.distance - skinWidth) * directionY;
					rayLength = hit.distance;
				}
				if (collisions.climbingSlope) {
					velocity.x = velocity.y / Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign (velocity.x);
				}

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
				if (me.IsDead ()) {
					velocity.y = 0;
					continue;
				}
				if (collisions.below  && (fightingMask.value & 1 << collisionLayer) != 0) {		//Jumped/Bounced on Player - Inflict Damage?
					if ((me.transform.position.y - hit.transform.GetComponent<Player> ().transform.position.y) > 2.1f) {
						me.velocity.y = me.jumpVelocity;
						hit.transform.GetComponent<Player> ().velocity.y = -10; 	//Add implementation of Dictionary Here
						hit.transform.GetComponent<Player> ().health -= 20;
						Move (me.velocity * Time.deltaTime);
					} else {
						velocity.y = 0;
					}
				}
			}
		}

		if (collisions.climbingSlope) {
			float directionX = Mathf.Sign (velocity.x);
			rayLength = Mathf.Abs (velocity.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) +Vector2.up*velocity.y;
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			if (hit) {
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);

				if (slopeAngle != collisions.slopeAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}

	void ClimbSlope(ref Vector3 velocity, float slopeAngle){

		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY) {
			velocity = new Vector3 (Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x), climbVelocityY);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}

	void DescendSlope(ref Vector3 velocity){
		float directionX = Mathf.Sign (velocity.x);
		Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

		if (hit) {
			float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);

			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {

				if (Mathf.Sign(hit.normal.x) == directionX) {

					if(hit.distance -skinWidth <= Mathf.Tan(slopeAngle *Mathf.Deg2Rad) *Mathf.Abs(velocity.x)){
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
						velocity.y -= descendVelocityY;

						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true;
					}
				}
			}
		}
	}

	//This is How to Inflict Damage and Affect Other players with a punch
	public void Punch(float directionX){ 
		float rayLength = punchLength / 10;
		Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.fistLeft : raycastOrigins.fistRight);
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, fightingMask);

		Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

		//This is used to obtain
		if (hit) {
			Player enemy = hit.transform.GetComponent<Player>(); //Create Player Dictionary
			enemy.velocity = (new Vector3(punchForce * directionX, 15));	
			enemy.health -= 10;
		}

		//Item Pick Up

		for (int i = 0; i <  pickUpRayCount; i++) {
			rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (pickUpRaySpacing * i);

			hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, pickUpMask); 

			Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.black); //Draw Red Lines in Scene for Debuging Purposes

			if (hit) { //Case Ray Hits Considered Target
				print("h");
				Item item = hit.transform.GetComponent<Item>();
				me.PickUpItem (item);
				item.CatchPlayer (me);
				//item.grabbable = false;
			}
		}
	}

	public void FallThrough(){
		if (collisions.droppable) {
			transform.Translate (new Vector3 (0, -.5f, 0));
		}
	}
	public struct CollisionInfo{
		public bool above, below;
		public bool left, right;

		public bool climbingSlope,descendingSlope;
		public float slopeAngle, slopeAngleOld;
		public Vector3 velocityOld;
		public bool droppable;

		public void Reset(){
			above = below = false;
			left = right = false;
			climbingSlope = descendingSlope = false;
			droppable = false;
			slopeAngleOld = slopeAngle;
			slopeAngle = 0;

		}
	}
}

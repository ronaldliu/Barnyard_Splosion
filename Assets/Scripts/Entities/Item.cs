using UnityEngine;
using System.Collections;

public class Item : RaycastController {
	Vector3 position;
	Vector3 rotation;
	public bool held;
	public Player holdingMe;
	public LayerMask fightingMask;
	public int facing = 1;
	Vector2 aim;
	public Vector3 velocity;
	public float gravity = -10;
	SimpleCollision itemCollisions;
	public Transform childTransform;
	bool rotate = true;
	void Awake () {
		//print ("Gun");
		childTransform = transform.FindChild("Image");
		collider = childTransform.GetComponent<BoxCollider2D>();
		itemCollisions.previousTime = 0;
	}
	void Update(){
		if (held) {
			//AlignItem();
		} else {
			if (itemCollisions.above || itemCollisions.below) {
				velocity = Vector3.zero;
				rotate = false;
				itemCollisions.previousTime = Time.time;
			} else {
				if (Time.time - itemCollisions.previousTime > .1f || itemCollisions.previousTime == 0) {
					rotate = true;
				}
			}
			Rotation ();
			UpdateItemRaycastOrigins ();
			itemCollisions.Reset ();
			itemCollisions.velocityOld = velocity;
			//transform.Rotate (0,0,10);
			//print (-90 - transform.localRotation.eulerAngles.z);
				
			
			velocity.y += (gravity  * Time.deltaTime);
			velocity = velocity ;
			Move (velocity * Time.deltaTime);
			///new Vector3(Mathf.Cos(Mathf.Deg2Rad * (-90 - transform.localRotation.eulerAngles.z)),
				//Mathf.Sin(Mathf.Deg2Rad * (-90 - transform.localRotation.eulerAngles.z)) )*.001f);			// Gravity, Physics, Things of that nature
			
		}
	}
	public virtual void Fire(){
		print ("Fire!");
	}
	public void CatchPlayer(Player me){
		holdingMe = me;
		transform.SetParent(holdingMe.transform);
		fightingMask = me.controller.fightingMask;
		held = true;
	}
	public void PlayerDrop(){
		holdingMe = null;
		itemCollisions.Reset ();
		fightingMask = new LayerMask();
		transform.SetParent (null);
		held = false;
		transform.rotation = Quaternion.Euler(0,0,0);
		childTransform.rotation = Quaternion.Euler(0,0,Mathf.Round(childTransform.rotation.eulerAngles.z/ 5) * 5);
		//facing = 1;
		//transform.localScale = new Vector3 (facing, 1);
	}
	public void Move(Vector3 velocity){
		if (velocity.x != 0) {
			HorizonatalCollisions (ref velocity);
		}
		UpdateItemRaycastOrigins ();

		//Only Collide Vertically if Moving So
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}
		transform.Translate (velocity );
	}
	void Rotation(){
		float current = childTransform.localRotation.eulerAngles.z;
		current = facing == 1 ? current : current > 180 ? 180 + 360 - current : 180 - current;
		if (current % 5 != 0) {
		}
		if (rotate) {
			childTransform.Rotate (0, 0, 10);
		} else if ((current > 0.1f && current < 60) || (current > 90.1f && current <= 120) || (current > 180.1f && current < 270)) {
			childTransform.Rotate (0, 0, -5 * facing);
		} else if ((current >= 60 && current < 89.9f) || (current >120&& current < 179.9f) || (current >= 270 && current < 359.9f)) {
			childTransform.Rotate (0, 0, 5 * facing);
		}
	}
	public virtual void AlignItem(){
		facing =(int) holdingMe.facing;
		transform.localScale = new Vector3(facing, 1);
		transform.position = new Vector3 ( holdingMe.transform.position.x + holdingMe.weap.WorldX , holdingMe.transform.position.y +holdingMe.weap.WorldY);
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, (holdingMe.facing > 0 ) ? (holdingMe.arm.rotation + 150) : 360 - (holdingMe.arm.rotation + 150)));
		childTransform.rotation = Quaternion.Euler (new Vector3 (0, 0, (holdingMe.facing > 0 ) ? (holdingMe.arm.rotation + 150) : 360 - (holdingMe.arm.rotation + 150)));

	}
	void HorizonatalCollisions(ref Vector3 velocity){
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		//Loop For cycling through Cast Rays Up the Side of the Character
		for (int i = 0; i < 2; i++) {
			Vector2 rayOrigin = Vector2.zero;
			switch (i) {
			case 0:
				rayOrigin = directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
				break;
			case 1:
				rayOrigin = directionX == -1 ? raycastOrigins.topLeft : raycastOrigins.topRight;
				break;
			}
			//Raycast Detection of Collisions with only collisionMask Layer being considered
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask); 
			RaycastHit2D playerHit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, fightingMask); 

			Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.white); //Draw Red Lines in Scene for Debuging Purposes

			if (hit) { //Case Ray Hits Considered Target
				int collisionLayer = hit.transform.gameObject.layer;

				if (LayerMask.NameToLayer ("Platforms") == collisionLayer) { //if inside of object allow player to move freely
					continue;
				}
				this.velocity.x *= - 1;
				velocity.x = 0;
				transform.Translate (this.velocity.x * Time.deltaTime, 0, 0);
				//velocity.x = (hit.distance - skinWidth) * directionX;

				itemCollisions.left = directionX == -1;
				itemCollisions.right = directionX == 1;
				break;
			} else if (playerHit && false) {
				Player enemy = playerHit.transform.GetComponent<Player> ();
				//velocity.x = (hit.distance - skinWidth) * directionX;
				if (playerHit.distance == 0 && false) {
					if ((transform.position.x - enemy.transform.position.y) > 0) {
						velocity.x = 5 / 4.25f;
					} else if ((transform.position.x - enemy.transform.position.y) <= 0) {
						velocity.x = -5 / 4.25f;
					}
				}
				rayLength = hit.distance;
				itemCollisions.left = directionX == -1;
				itemCollisions.right = directionX == 1;
				if (Mathf.Abs(enemy.velocity.x) < Mathf.Abs(velocity.x) && !enemy.IsDead()) {
					//enemy.controller.Move(new Vector3(velocity.x,0,0));
					//break;
				}


			}
		}
	}
	void VerticalCollisions(ref Vector3 velocity){
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y)+ skinWidth;

		for (int i = 0; i < 2; i++) {
			Vector2 rayOrigin = Vector2.zero;
			switch (i) {
			case 0:
				rayOrigin = directionY == -1 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
				break;
			case 1:
				rayOrigin = directionY == -1 ? raycastOrigins.bottomRight : raycastOrigins.topRight;
				break;
			}
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if (hit) {
				int collisionLayer = hit.transform.gameObject.layer;

				if (directionY > 0 && LayerMask.NameToLayer ("Platforms") == collisionLayer) {
					continue;
				}

				if (directionY == -1) {
					velocity.y = (hit.distance - skinWidth) * directionY;
					rayLength = hit.distance;
				}


				itemCollisions.below = directionY == -1;
				itemCollisions.above = directionY == 1;


			}
		}
	}
	public void UpdateItemRaycastOrigins(){
		collider = childTransform.GetComponent<BoxCollider2D>();
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight= new Vector2 (bounds.max.x, bounds.max.y);
	}
		public struct SimpleCollision{
			public bool above, below;
			public bool left, right;
	
			public Vector3 velocityOld;
			public Vector3 gravityOld;
			public float previousTime;
			public void Reset(){
				above = below = false;
				left = right = false;
			}
	}

}
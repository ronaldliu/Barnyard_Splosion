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
			AlignItem();
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

		VerticalCollisions (ref velocity);
		transform.Translate (velocity);
	}
	void Rotation(){
		float current = childTransform.localRotation.eulerAngles.z;
		current = facing == 1 ? current : current > 180 ? 180 + 360 - current : 180 - current;
		if (current % 5 != 0) {
		}
		if (rotate) {
			childTransform.Rotate (0, 0, 10);
		} else if ((current > 0.1f && current < 60) || (current > 90.1f && current <= 120) || (current > 180.1f && current < 270)) {
			childTransform.Rotate (0, 0, -5*facing);
		} else if ((current >= 60 && current < 89.9f) || (current >120&& current < 179.9f) || (current >= 270 && current < 359.9f)) {
			childTransform.Rotate (0, 0, 5*facing);
		}
	}
	public virtual void AlignItem(){
		facing =(int) holdingMe.facing;
		transform.localScale = new Vector3(facing, 1);
		transform.position = new Vector3 ( holdingMe.transform.position.x + holdingMe.weap.WorldX , holdingMe.transform.position.y +holdingMe.weap.WorldY);
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, (holdingMe.facing > 0 ) ? (holdingMe.arm.rotation + 150) : 360 - (holdingMe.arm.rotation + 150)));
		childTransform.rotation = Quaternion.Euler (new Vector3 (0, 0, (holdingMe.facing > 0 ) ? (holdingMe.arm.rotation + 150) : 360 - (holdingMe.arm.rotation + 150)));

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

				if (collisionLayer == LayerMask.NameToLayer ("Platforms") || directionY == -1) {
					velocity.y = (hit.distance - skinWidth) * directionY;
					rayLength = hit.distance;
					//print ("Hit " + LayerMask.LayerToName(collisionLayer));
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
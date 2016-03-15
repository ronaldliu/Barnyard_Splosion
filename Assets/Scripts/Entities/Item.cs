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
	Vector3 velocity;
	Vector3 gravity = Vector3.zero;
	SimpleCollision itemCollisions;
	public Transform childTransform;
	void Start () {
		//print ("Gun");
		childTransform = transform.FindChild("Image");
		itemCollisions.previousDelta = 0;
	}
	void Update(){
		if (held) {
			AlignItem();
		} else {
			if (itemCollisions.above || itemCollisions.below) { 		
				velocity = Vector3.zero;
			} else {
				childTransform.Rotate (0, 0, 1);
			}
			UpdateRaycastOrigins ();
			itemCollisions.Reset ();
			itemCollisions.velocityOld = velocity;
			itemCollisions.gravityOld = gravity;
			//transform.Rotate (0,0,10);
			velocity -= itemCollisions.gravityOld *itemCollisions.previousDelta;
			//print (-90 - transform.localRotation.eulerAngles.z);
			//Debug.DrawRay (transform.position,  new Vector3(Mathf.Cos(Mathf.Deg2Rad * (-90 - transform.localRotation.eulerAngles.z)),
				//Mathf.Sin(Mathf.Deg2Rad * (-90 - transform.localRotation.eulerAngles.z)) ) ); // - new Vector3 (Mathf.Sin (Mathf.Deg2Rad * transform.eulerAngles.z),Mathf.Cos (Mathf.Deg2Rad * transform.eulerAngles.z)));

			gravity = (new Vector3 (Mathf.Cos (Mathf.Deg2Rad * (-90 - transform.localRotation.eulerAngles.z)),
				Mathf.Sin (Mathf.Deg2Rad * (-90 - transform.localRotation.eulerAngles.z))) * .005f);
			gravity += gravity * (itemCollisions.gravityOld.magnitude / gravity.magnitude);
			
			velocity += (gravity  * Time.deltaTime);
			itemCollisions.previousDelta = Time.deltaTime;
			VerticalCollisions (ref velocity);
			transform.Translate (velocity);///new Vector3(Mathf.Cos(Mathf.Deg2Rad * (-90 - transform.localRotation.eulerAngles.z)),
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
		held = false;
	}

	public virtual void AlignItem(){
		facing =(int) holdingMe.facing;
		transform.localScale = new Vector3(facing, 1);
		transform.position = new Vector3 ( holdingMe.transform.position.x + holdingMe.weap.WorldX , holdingMe.transform.position.y +holdingMe.weap.WorldY);
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, (holdingMe.facing > 0 ) ? (holdingMe.arm.rotation + 150) : 360 - (holdingMe.arm.rotation + 150)));

	}
	void VerticalCollisions(ref Vector3 velocity){
		Vector3 projVector = Vector3.Project (velocity, new Vector3 (Mathf.Cos (Mathf.Deg2Rad * (-90 - transform.localRotation.eulerAngles.z)),
			                     Mathf.Sin (Mathf.Deg2Rad * (-90 - transform.localRotation.eulerAngles.z))));
		float directionY =  -1;
		print ("Dir " + directionY);
		float rayLength = projVector.magnitude + skinWidth; //Mathf.Abs (Vector3.Project(velocity,new Vector3(Mathf.Cos(Mathf.Deg2Rad * (-90 - transform.localRotation.eulerAngles.z)),
			//Mathf.Sin(Mathf.Deg2Rad * (-90 - transform.localRotation.eulerAngles.z)) )).magnitude)+ skinWidth;

		for (int i = 0; i < 4; i++) {
			Vector2 rayOrigin = Vector2.zero;
			switch (i) {
			case 0:
				rayOrigin = raycastOrigins.bottomLeft;
				break;
			case 1:
				rayOrigin = raycastOrigins.bottomRight;
				break;
			case 2:
				rayOrigin = raycastOrigins.topLeft;
				break;
			case 3:
				rayOrigin = raycastOrigins.topRight;
				break;
			}
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin,  Vector3.up * directionY, rayLength, collisionMask);

			Debug.DrawRay (rayOrigin, Vector3.up * directionY * rayLength, Color.red);

			if (hit) {
				print ("Hittin it");
				int collisionLayer = hit.transform.gameObject.layer;

				if (directionY > 0 && LayerMask.NameToLayer ("Platforms") == collisionLayer) {
					continue;
				}

				if (collisionLayer == LayerMask.NameToLayer ("Platforms") || directionY == -1) {
					//velocity = (velocity - projVector) + ((projVector/projVector.magnitude)*((hit.distance - skinWidth) * directionY));
					rayLength = hit.distance;
				}

				itemCollisions.below = directionY == -1;
				itemCollisions.above = directionY == 1;
			}
		}
	}
		public struct SimpleCollision{
			public bool above, below;
			public bool left, right;
	
			public Vector3 velocityOld;
			public Vector3 gravityOld;
			public float previousDelta;
			public void Reset(){
				above = below = false;
				left = right = false;
			}
	}

}
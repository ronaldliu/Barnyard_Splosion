using UnityEngine;
using System.Collections;

[RequireComponent(typeof (BoxCollider2D))]
public class RaycastController : MonoBehaviour {

	//Editor Customizable
	public const float skinWidth = 0.025f/4.75f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;
	public int pickUpRayCount = 3;
	public LayerMask collisionMask;			//Used to Specify layers to Collide With //CREATE LAYER FOR EACH PLAYER AND COMBINE WITH COLLISONMASK -- Set this value on startin Player and platform
	public RaycastOrigins raycastOrigins; 	//Struct for storing Raycast Data

	//Non-Editor Customizable, but Still Visible from Other Classes
	[HideInInspector]
	public float horizontalRaySpacing;	
	[HideInInspector]
	public float verticalRaySpacing;
	[HideInInspector]
	public float pickUpRaySpacing;
	[HideInInspector]
	public new BoxCollider2D collider;

	public virtual void Awake() {
		collider = GetComponent<BoxCollider2D> ();
	}

	public virtual void Start() {
		CalculateRaySpacing ();
	}
	//Sets the Raycast Data to Correct Locations Based on the Location of the Character
	public void UpdateRaycastOrigins(){
		collider = GetComponent<BoxCollider2D> ();
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight= new Vector2 (bounds.max.x, bounds.max.y);
		raycastOrigins.fistRight= new Vector2 (bounds.max.x, (bounds.max.y - (bounds.max.y -bounds.min.y)/3.65f));
		raycastOrigins.fistLeft= new Vector2 (bounds.min.x, (bounds.max.y - (bounds.max.y -bounds.min.y)/3.65f));


	}

	//Evenly Spaces Raycastings
	public void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);
		pickUpRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
		pickUpRaySpacing = (bounds.size.y/2) / (verticalRayCount - 1);

	}

	public struct RaycastOrigins{
		public Vector2 topLeft,topRight;
		public Vector2 bottomLeft,bottomRight;
		public Vector2 fistRight,fistLeft;
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (SkeletonAnimation))]
public class Player : MonoBehaviour {
    //Temporary fix for the animation
	private bool animReset = true;

	//Editor Customizable
	public float jumpHeight = 6/4.75f;
	public float timeToJumpApex = .45f;
	public float moveSpeed = 6/4.75f;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .1f;
	public string player = "P1";  	//This is for Multiplayer Support
	public float health = 100;
	public bool dead = false;
	public Sprite image;
	public float aimAngle;

	public float facing = 1;
	public float gravity;
	[HideInInspector]
	public float jumpVelocity;
	public Vector3 velocity;
	float velocityXSmoothing;
	[HideInInspector]
	public BoxCollider2D boxCollider;

	//Class References
	SkeletonAnimation anim;
	MeshRenderer character;
	Controller2D controller;
	Spine.SkeletonData skeletonData;
	public Spine.Skeleton skeleton;
	public Spine.Bone arm;
	Spine.Bone backArm;
	public Spine.Bone weap;
	public SkeletonRenderer skelRend;
	Item holding;
	TapInfo crouchTap;

	//ItemEntity item;
	//Class Reference to Item Entity Here!

	void Start () {
		boxCollider = GetComponent<BoxCollider2D> ();
		character = GetComponent<MeshRenderer> ();
		controller = GetComponent<Controller2D> ();
		anim = GetComponent<SkeletonAnimation> ();
		skeleton = anim.skeleton;
		arm = skeleton.FindBone ("RShoulder");
		backArm = skeleton.FindBone ("LShoulder");

		weap = skeleton.FindBone ("Weapon");
		skelRend = GetComponent<SkeletonRenderer> ();
		skeleton.FindSlot ("WeaponImage").Attachment = null;

		//skeleton.FindSlot ("WeaponImage").A = 0;
		//skelRend.skeleton.AttachUnitySprite ("WeaponImage", image);
		//skelRend.skeleton.AttachUnitySprite ("Art/Weapons/LMG", image);
		anim.state.ClearTrack(1);
		controller.CatchPlayer (this);
		crouchTap = new TapInfo (.6f, 3);
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
			aimAngle = 0;
			if (!aimer.Equals (Vector2.zero)) {
				if (Mathf.Abs(aimer.x) < 0.1f) {
					if (Mathf.Sign (aimer.y) == -1) {
						aimAngle = 270 * Mathf.Deg2Rad;
					} else {
						aimAngle = 90 * Mathf.Deg2Rad;
					}
				} else if (aimer.y == 0) {
					facing = Mathf.Sign (aimer.x);
				} else {
					aimAngle = Mathf.Atan2(aimer.y, Mathf.Abs(aimer.x));
					facing = Mathf.Sign (aimer.x);
				}
			}

			//Sprite Direction
			if(input.x != 0 && Mathf.Abs(aimer.x) < .1f) {
				facing = Mathf.Sign (input.x);
			}
			arm.rotation = Mathf.Rad2Deg * (aimAngle) - 150; 
			backArm.rotation = Mathf.Rad2Deg * (aimAngle) - 150; 

			weap.rotation = -55; 
			//character.transform.localScale = new Vector3 (facing  , 1, 0);
			skeleton.flipX = facing<0;
			//character.transform.localScale = new Vector3 (facing * 4.75f , 4.75f, 0);

			Debug.DrawRay(character.transform.position,new Vector3(aimAngle == 90 || aimAngle == 270 ? 0  : (Mathf.Abs(Mathf.Cos(aimAngle))*facing),Mathf.Sin(aimAngle) , 0)* 5,Color.cyan);
			
			if (input.y < -0.5f && Mathf.Abs (input.x) < 0.05f) {
				if (crouchTap.TapCheck () && !crouchTap.activeDTap) {
					controller.FallThrough ();
					crouchTap.activeDTap = true;
				}
			} else {
				crouchTap.Reset ();
			}

			//Jump Velocity
			if (Input.GetButtonDown ("Jump_" + player) && controller.collisions.below) {
				velocity.y = jumpVelocity;
			}

            //jump Animation
            if(velocity.y != 0)
            {
                anim.state.SetAnimation(2, "Jump", false);
            }
			//Hit/Fire Weapon
			if (Input.GetButtonDown ("Fire_" + player) && holding == null) {
				//Add Weapon Fire Support Here
				anim.state.SetAnimation (3, "Poke", false);
				controller.Punch (facing);
			} else if (Input.GetButton("Fire_" + player) && holding != null) {
				holding.Fire ();
				print ("Pow");
			} 
			//Horizontal Velocity Smoothing

			float targetVelocityX = input.x * moveSpeed;
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing,
				(controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

			if (targetVelocityX != 0) {
				if (animReset)
                {
					animReset = false;
                    anim.state.SetAnimation(1, "animation", true);
                }
			} else {
				anim.state.SetAnimation (1, "Standing", true);
				anim.state.ClearTrack(1);

                //Reset the walk animation
                animReset = true;
			}
		
		
		} else {
			if (!dead) { 
				Death ();
			}
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
		boxCollider.size = new Vector2 (.55f, .22f);
		boxCollider.offset = new Vector2 (-.06f, -.164f);
		gameObject.layer = 14;
		controller.FallThrough ();
		dead = true;
	}

	public void PickUpItem(Item item){
		holding = item;
		image = item.GetComponent<SpriteRenderer> ().sprite;
		skelRend.skeleton.AttachUnitySprite ("WeaponImage", image,"Sprites/Default");
		print ("Pick Up Successful");
		//Add to skeleton here
	}
	//This Method is for Converting the More Intuitive Jump Height and Time
	//Variables into Gravity and Jump Velocity to Use on Character
	//Calculations Based on Physics
	public void UpdateGravity(){
		gravity = -(jumpHeight * 2) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs (gravity) * timeToJumpApex;
	}
	public struct TapInfo{
		public bool lastInput;
		public bool activeDTap;
		public int tapCount;
		public float lastTap;
		public float delay;
		public int maxTaps;

		public TapInfo(float delay,int maxTaps){
			this.delay = delay;
			this.maxTaps = maxTaps;
			lastInput = false;
			activeDTap = false;
			tapCount = 0;
			lastTap = 0;
		}
		public bool TapCheck(){
			if (!lastInput) {
				lastTap = Time.time;
				lastInput = true;
				tapCount++;
			}
			if (tapCount > 1 && tapCount <= maxTaps) {
				return true;
			}
			return false;
		}
		public void Reset(){
			activeDTap = false;
			lastInput = false;
			if (Time.time - lastTap > delay) {
				tapCount = 0;
			}
		}
	}
}

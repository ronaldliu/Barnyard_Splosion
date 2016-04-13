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
	[HideInInspector]
	public bool weaponInHand = false;
	public bool arms = true;
	public float facing = 1;
	public float gravity;
	[HideInInspector]
	public float jumpVelocity;
	public Vector3 velocity;
	float velocityXSmoothing;
	[HideInInspector]
	public BoxCollider2D boxCollider;
	[HideInInspector] 
	public int killCount = 0;

	//Class References
	SkeletonAnimation anim;
	MeshRenderer character;
	public Controller2D controller;
	Spine.SkeletonData skeletonData;
	public Spine.Skeleton skeleton;
	public Spine.Bone arm;
	Spine.Bone backArm;
	public Spine.Bone weap;
	public SkeletonRenderer skelRend;
	Item holding;
	TapInfo crouchTap;
	bool shotLock = true;
	GameHandler game;
	TapInfo dashTap;
    //ItemEntity item;
    //Class Reference to Item Entity Here!

    //for the arm rotation for HoldingRifle
    public float FrontArmRotation = 217.67f;
    private float BackArmRotation = 251.08f;
    private float WeaponWorldRotation = 131.94f;

    //For the regular Arm holding

    private float FrontArmRotationNoWeapon = 222.44f;
    private float BackArmRotationNoWeapon = 222.44f;

    //This is for healthbar, getting a refernece to the Canvas GameObject, to access it's child RawImage
    public GameObject healthbar;
    //These will be for storing the distance between the RawImage recttransform left and right values
    private float width;
    private float startMaxXPos;

    void Start () {
		game = GameObject.Find ("Gui").GetComponent<GameHandler> ();
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
		anim.state.ClearTrack(1);
		controller.CatchPlayer (this);
		crouchTap = new TapInfo (.6f, int.MaxValue);
		dashTap = new TapInfo (.6f, int.MaxValue);

        //Initiate the width of the HP bar, this may need to be placed in the Update portion if window scaling is changed.
        width = healthbar.GetComponent<RectTransform>().rect.width;
        startMaxXPos = healthbar.GetComponent<RectTransform>().offsetMax.x;
        //print(width + " " + startMaxXPos);

        UpdateGravity();
	}

	void Update(){

        //Change the width of the HPbar in accordance to the ration of current health to max health
        healthbar.GetComponent<RectTransform>().offsetMax = new Vector2(startMaxXPos - width * (1 - (health / 100)), healthbar.GetComponent<RectTransform>().offsetMax.y);
        //print(width + " " + health);

        UpdateGravity();
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal_" + player), Input.GetAxisRaw ("Vertical_" + player));
		Vector2 aimer = new Vector2 (Input.GetAxisRaw ("AimH_" + player), Input.GetAxisRaw ("AimV_" + player));

		//This is a fix for velocity accumulation while on the ground
		if (controller.collisions.above || controller.collisions.below) { 		
			velocity.y = 0;
		}
		if (!game.gameEnd) {
			anim.enabled = true;
			if (!IsDead ()) {
				aimAngle = 0;
				if (!aimer.Equals (Vector2.zero)) {
					if (Mathf.Abs (aimer.x) < 0.1f) {
						if (Mathf.Sign (aimer.y) == -1) {
							aimAngle = 270 * Mathf.Deg2Rad;
						} else {
							aimAngle = 90 * Mathf.Deg2Rad;
						}
					} else if (aimer.y == 0) {
						facing = Mathf.Sign (aimer.x);
					} else {
						aimAngle = Mathf.Atan2 (aimer.y, Mathf.Abs (aimer.x));
						facing = Mathf.Sign (aimer.x);
					}
				}

				//Sprite Direction
				if (input.x != 0 && Mathf.Abs (aimer.x) < .1f) {
					facing = Mathf.Sign (input.x);
				}
                if (arms)
                {
                    //arm.rotation = Mathf.Rad2Deg * (aimAngle) - 150; 
                    //backArm.rotation = Mathf.Rad2Deg * (aimAngle) - 150; 
                    // This is currently only set up for the holding rifle animation
                    if (weaponInHand)
                    {
                        //Check to see what weapon type it is and apply the appropriate rotations
                        if (holding.weaponType == "Rifle")
                        {
                            arm.rotation = FrontArmRotation + (aimAngle * 180 / Mathf.PI);
                            backArm.rotation = BackArmRotation + (aimAngle * 180 / Mathf.PI);
                            weap.rotation = -31;
                            weap.x = -.020f;
                        }
                        else if (holding.weaponType == "Longsword")
                        {
                            arm.rotation = FrontArmRotationNoWeapon + Mathf.Rad2Deg * (aimAngle);
                            backArm.rotation = BackArmRotationNoWeapon + Mathf.Rad2Deg * (aimAngle);
                            weap.rotation = -70;
                        }

                    }
                    else
                    {
                        arm.rotation = FrontArmRotationNoWeapon + Mathf.Rad2Deg * (aimAngle);
                        backArm.rotation = BackArmRotationNoWeapon + Mathf.Rad2Deg * (aimAngle);
                    }
                }
                skeleton.flipX = facing < 0;
				Debug.DrawRay (character.transform.position, new Vector3 (aimAngle == 90 || aimAngle == 270 ? 0 :
				(Mathf.Abs (Mathf.Cos (aimAngle)) * facing), Mathf.Sin (aimAngle), 0) * .5f, Color.cyan);
			


				//Jump Velocity
				if ((Input.GetButton("Jump_" + player) || Input.GetAxisRaw ("Vertical_" + player) > .85f) ){//&& controller.collisions.below) {
					controller.LadderCheck ();
					//velocity.y = jumpVelocity;
				}

				//jump Animation
				if (velocity.y != 0) {
					anim.state.SetAnimation (2, "Jump", false);
				}
				//Hit/Fire Weapon
				if (weaponInHand) {
					holding.AlignItem ();
				}
				if (Input.GetButtonDown ("Fire_" + player) && !weaponInHand) {
					//Add Weapon Fire Support Here
					anim.state.SetAnimation (3, "Poke", false);
					controller.Punch (facing);
				} else if ((Input.GetButton ("Fire_" + player) || Input.GetAxisRaw ("Fire_" + player) > .25f) && weaponInHand) {
					holding.Fire ();
				} 
				//Drop Weapon
				if (Input.GetButtonDown ("Drop_" + player)) {
					if (weaponInHand) {
						DropItem ();

					} else {
						controller.PickupCheck ();
					}
				}

				//Dash implemented here
				float targetVelocityX = 0; 
				if (input.x != 0) {
					if (dashTap.TapCheck () && (Mathf.Sign (input.x) == dashTap.moving)) {
						targetVelocityX = input.x * moveSpeed * 2;
						if (animReset) {
							animReset = false;
							anim.state.ClearTrack (1);
							anim.state.SetAnimation (1, "Run", true);
						}
					} else {
						targetVelocityX = input.x * moveSpeed; 
						if (!dashTap.TapCheck ()) {
							dashTap.moving = Mathf.Sign (input.x);
						}
					}
				} else {
					dashTap.Reset ();
				}
				//Horizontal Velocity Smoothing

				velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing,
					(controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

				if (targetVelocityX != 0) {
					if (animReset) {
						animReset = false;
						anim.state.ClearTrack (1);
						anim.state.SetAnimation (1, "Walking", true);
					}
				} else {
					anim.state.SetAnimation (1, "Standing", true);
					anim.state.ClearTrack (1);

					//Reset the walk animation
					animReset = true;
				}
				if (input.y < -0.5f && Mathf.Abs (input.x) < 0.05f) {
					anim.state.ClearTrack (1);

					anim.state.SetAnimation (1, "Crouch", true);
					if (crouchTap.TapCheck () && !crouchTap.activeDTap) {
						controller.FallThrough ();
						crouchTap.activeDTap = true;
					}
				} else {
					crouchTap.Reset ();
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
		} else {
			anim.enabled = false;
		}
	}

	public bool IsDead(){
		return health <= 0;
	}

	//Drop Item, Animate Death, Ect
	public void Death(){
		print (player);
		if (weaponInHand) {
			DropItem ();
		}
		anim.state.ClearTracks ();
		anim.state.SetAnimation (1, "Death", false);
		boxCollider.size = new Vector2 (.55f, .22f);
		boxCollider.offset = new Vector2 (-.06f, -.164f);
		gameObject.layer = 14;
		controller.FallThrough ();
		dead = true;
	}
	public void DropItem(){
		if (! new Vector2 (Input.GetAxisRaw ("AimH_" + player), Input.GetAxisRaw ("AimV_" + player)).Equals(Vector2.zero)) {
			holding.velocity = new Vector3 (aimAngle == 90 || aimAngle == 270 ? 0 :
				(Mathf.Abs (Mathf.Cos (aimAngle)) * facing), Mathf.Sin (aimAngle), 0)*5;
		}
		boxCollider.size = new Vector2 (.23f, .55f);

        //This code resets arm rotations to the default punching stance
        anim.state.SetAnimation(2, "Arms-Reset", false);

        skeleton.FindSlot ("WeaponImage").Attachment = null;
		holding.PlayerDrop();
		holding = null;
		weaponInHand = false;
	}
	public void PickUpItem(Item item){
		anim.state.ClearTrack (3);
		boxCollider.size = new Vector2 (.45f, .55f);		
		boxCollider.offset = new Vector2 (0, 0);
		holding = item;
		image = item.childTransform.GetComponent<SpriteRenderer> ().sprite;
		skelRend.skeleton.AttachUnitySprite ("WeaponImage", image,"Sprites/Default");

        //This code will need to be changed to be a conditional, based on the type of item picked up.
        //This code is now conditional, however, the case where the item picked up is not a weapon (or doesnt have that variable) 
        // Is not handled currently.
        if (item.isWeapon)
        {
            if (item.weaponType == "Rifle")
            {
                anim.state.SetAnimation(2, "HoldingRifle", false);
            }
            else if (item.weaponType == "Longsword")
            {
                anim.state.SetAnimation(2, "Arms-Reset", false);
            }
        }

        weaponInHand = true;
		shotLock = true;
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
		public float moving;
		public TapInfo(float delay,int maxTaps){
			this.delay = delay;
			this.maxTaps = maxTaps;
			lastInput = false;
			activeDTap = false;
			tapCount = 0;
			lastTap = 0;
			moving = 0;
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

using UnityEngine;
using System.Collections;

public class Projectile : RaycastController {
	WeaponController AttachedTo;
	bool gravity;
	float speed;
	float damage;
	LayerMask fightingMask;
	float birthTime;
	int facing;
	Vector3 shootdir;
	Vector3 velocity;
	bool hitSomething = false;

	// Use this for initialization
	void Start()
	{
		birthTime = Time.time;
		gameObject.layer = LayerMask.NameToLayer ("Item");

	}
		
	public void SetupProjectile(LayerMask collisionMask,LayerMask fightingMask, WeaponController AttachedTo,float speed,float damage,int facing, bool gravity){
		this.collisionMask = collisionMask;
		this.fightingMask = fightingMask;
		this.AttachedTo = AttachedTo;
		this.speed = speed;
		this.damage = damage;
		this.gravity = gravity;
		this.facing = facing;
		shootdir = Vector3.right * facing;
		velocity = shootdir * speed/50;
		//shootdir.y += Random.Range (-AttachedTo.variance/20, AttachedTo.variance/20);
		//print ("S " + speed);

	}

	void Update()
	{

		if (!gravity) {//Finish gravity calcs
			transform.Translate (velocity);
		} else {
			// For use with gravity
			velocity.y -= .005f;
			transform.Translate (velocity);
		}

		float rayLength = Mathf.Abs(velocity.magnitude) + skinWidth;
		Vector2 rayOrigin = ((facing == -1) ? raycastOrigins.fistLeft : raycastOrigins.fistRight);
		RaycastHit2D hit = Physics2D.Raycast (transform.position, transform.right * facing , rayLength, collisionMask + fightingMask);

		Debug.DrawRay (transform.position, transform.right * facing * rayLength, Color.magenta);

		if (hit)
		{

			velocity.x = (hit.distance - skinWidth) * facing;
			if (hitSomething) {
				int collisionLayer = hit.transform.gameObject.layer;

				if ((fightingMask.value & 1 << collisionLayer) != 0) {
					Player enemy = hit.transform.GetComponent<Player> (); //Create Player Dictionary
					enemy.health -= 10;
					enemy.velocity = shootdir;
				} else if ((collisionMask.value & 1 << collisionLayer) == 0) {
					//Ricochette? 
					print("x " + shootdir.x +"y " + shootdir.y +"z " + shootdir.z );
					//shootdir = Vector3.Reflect(shootdir,Vector3.back);
				}
				Destroy (gameObject);
			}
			hitSomething = true;
		}
	}
}

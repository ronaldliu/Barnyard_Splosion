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
		shootdir = transform.right * facing;
		shootdir.y += Random.Range (-AttachedTo.variance/20, AttachedTo.variance/20);
		print ("S " + speed);

	}

	void Update()
	{
		if (!gravity) {//Finish gravity calcs
			transform.Translate (shootdir * Time.deltaTime * speed);
		} else {
			// For use with gravity
			transform.Translate (shootdir * Time.deltaTime * speed);
		}

		float rayLength = 0.06f;
		Vector2 rayOrigin = ((facing == -1) ? raycastOrigins.fistLeft : raycastOrigins.fistRight);
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.right * facing , rayLength, collisionMask + fightingMask);

		Debug.DrawRay (transform.position, Vector2.right * facing * rayLength, Color.magenta);

		if (hit)
		{
			int collisionLayer = hit.transform.gameObject.layer;
			if ((fightingMask.value & 1 << collisionLayer) != 0) {
				Player enemy = hit.transform.GetComponent<Player> (); //Create Player Dictionary
				enemy.health -= 10;
			} else if ((collisionMask.value & 1 << collisionLayer) == 0) {
				//Ricochette? 
			}
			Destroy (gameObject);
		}
	}
}

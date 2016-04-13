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
	Transform imageTransform;
	bool collision = true;
	// Use this for initialization
	void Start()
	{
		birthTime = Time.time;
		gameObject.layer = LayerMask.NameToLayer ("Item");

	}
		
	public void SetupProjectile(LayerMask collisionMask,LayerMask fightingMask, WeaponController AttachedTo,float speed,float damage,int facing,Vector3 shootdir, bool gravity){
		imageTransform = transform.FindChild ("Image").transform;
		collider = imageTransform.GetComponent<BoxCollider2D> ();
		this.collisionMask = collisionMask;
		this.fightingMask = fightingMask;
		this.AttachedTo = AttachedTo;
		this.speed = speed;
		this.damage = damage;
		this.gravity = gravity;
		this.facing = facing;
		this.shootdir = shootdir; //* facing;//Vector3.right * facing;
		this.shootdir *= facing;
		if (facing < 0) {
			//GetComponent<SpriteRenderer> ().flipX = true;
		}
		velocity = this.shootdir * speed/50;
		imageTransform.rotation =  Quaternion.Euler((this.shootdir.y < 0?-1:1) * new Vector3(0,0,Vector3.Angle(Vector3.right,this.shootdir)));
		//shootdir.y += Random.Range (-AttachedTo.variance/20, AttachedTo.variance/20);
		//print (imageTransform.rotation);;

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
		if (collision) {
			ProjectileCollision ();
		}

	}
	void ProjectileCollision(){
		float rayLength = Mathf.Abs(velocity.magnitude) + skinWidth;
		Vector2 rayOrigin = ((facing == -1) ? raycastOrigins.fistLeft : raycastOrigins.fistRight);
		RaycastHit2D hit = Physics2D.Raycast (transform.position, shootdir , rayLength, collisionMask);
		RaycastHit2D playerHit = Physics2D.Raycast (transform.position, transform.right * facing , rayLength, fightingMask);
		//print (shootdir);
		Debug.DrawRay (transform.position, shootdir * rayLength, Color.magenta);

		if (hit) {

			int collisionLayer = hit.transform.gameObject.layer;

			if (LayerMask.LayerToName (collisionLayer) == "Obsticle") {
				print (hit.normal);
				if (hit.normal.Equals( Vector2.right) || hit.normal.Equals( Vector2.down) || hit.normal.Equals( Vector2.left)) {
					collision = false;
					return;
				}
			}
			velocity = (hit.distance - skinWidth) * (shootdir / shootdir.magnitude);


			//Ricochette? 
			//print("x " + shootdir.x +"y " + shootdir.y +"z " + shootdir.z );
			float ricochetProb = Random.Range(0,100);
			if (ricochetProb > 70) {
				shootdir = Vector3.Reflect (shootdir, hit.normal);
				imageTransform.rotation = Quaternion.Euler ((this.shootdir.y < 0 ? -1 : 1) * new Vector3 (0, 0, Vector3.Angle (Vector3.right, this.shootdir)));
				velocity = shootdir * speed / 50;
			} else {
				Destroy (gameObject);
			}
		} else if (playerHit) {
			velocity = (playerHit.distance - skinWidth) * (shootdir / shootdir.magnitude);
			int collisionLayer = playerHit.transform.gameObject.layer;
			if (hitSomething) {


				Player enemy = playerHit.transform.GetComponent<Player> (); //Create Player Dictionary
				enemy.health -= 10;
				//enemy.velocity = shootdir;
				Destroy (gameObject);

			} 
			hitSomething = true;

		}
	}
}

using UnityEngine;
using System.Collections;

public class Projectile : RaycastController {
	public WeaponController AttachedTo;
	public bool gravity;
	Vector3 shootdir;
	// Use this for initialization
	void Start()
	{
		shootdir = transform.right * AttachedTo.holdingMe.facing;
		shootdir.y = Random.Range (-AttachedTo.variance/20, AttachedTo.variance/20);
	}

	void Update()
	{
		if (!gravity) {
			transform.Translate (shootdir * Time.deltaTime * AttachedTo.projectileSpeed);
			Destroy (gameObject, 1);
		} else {
			// For use with gravity
			transform.Translate (shootdir * Time.deltaTime * AttachedTo.projectileSpeed);
			Destroy (gameObject, 2);
		}

		float rayLength = (float) 0.06;
		Vector2 rayOrigin = ((AttachedTo.holdingMe.facing == -1) ? raycastOrigins.fistLeft : raycastOrigins.fistRight);
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.right * AttachedTo.holdingMe.facing , rayLength, collisionMask);

		Debug.DrawRay (transform.position, Vector2.right * AttachedTo.holdingMe.facing * rayLength, Color.magenta);

		if (hit)
		{
			// print ("I Hit A thing");
			Player enemy = hit.transform.GetComponent<Player>(); //Create Player Dictionary
			enemy.health -= 10;
			Destroy (gameObject);
		}
	}
}

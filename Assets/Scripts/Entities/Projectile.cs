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
	}
}

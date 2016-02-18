using UnityEngine;
using System.Collections;

public class Projectile : RaycastController {
	public WeaponController AttachedTo;
	public int speed;
	public bool gravity;
	Vector3 shootdir;
	// Use this for initialization
	void Awake()
	{
		shootdir = transform.right * AttachedTo.holdingMe.facing;
	}

	void Update()
	{
		if (!gravity) {
			transform.Translate (shootdir * Time.deltaTime * speed);
			Destroy (gameObject, 1);
		} else {
			// For use with gravity
			Vector3 dir = new Vector3 (Vector3.right.x, Vector3.right.y, Vector3.right.z);
			transform.Translate (dir * Time.deltaTime * speed);
			Destroy (gameObject, 2);
		}
	}
}

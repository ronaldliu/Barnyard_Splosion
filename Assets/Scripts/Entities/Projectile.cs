using UnityEngine;
using System.Collections;

public class Projectile : RaycastController {
	WeaponController AttachedTo;
	public int speed;
	public bool gravity;
	// Use this for initialization
	void Update()
	{
		if (!gravity) {
			transform.Translate (Vector3.right * Time.deltaTime * speed);
			Destroy (gameObject, 1);
		} else {
			Vector3 dir = new Vector3 (Vector3.right.x, Vector3.right.y, Vector3.right.z);
			transform.Translate (dir * Time.deltaTime * speed);
			Destroy (gameObject, 2);
		}
	}
}

using UnityEngine;
using System.Collections;


public class WeaponController : Item {
	
	public int ammo;
	// Higher the number the faster the gun shoots
	public float firerate;
	// The Bullet trail
	public Projectile projectile;
	public Transform shotSpawn;
	public float damage;
	public float projectileSpeed;
	Item item;
	// Used for bullet spread so it isnt just straight
	// Higher the number the more variance
	public float variance;

	float nextfire = 0;

	void Awake()
	{
		collider = GetComponent<BoxCollider2D> ();
		shotSpawn = transform.FindChild ("shotspawn");
		item = GetComponent<Item> ();
		projectile.AttachedTo = this;

		if (shotSpawn == null) 
		{
			Debug.LogError ("No shot spawn");
		}
	}

	public void Fire()
	{
		if (Time.time > nextfire && ammo != 0) 
		{
			nextfire = Time.time + 1 / firerate;
			ammo--;

			Instantiate (projectile, shotSpawn.position, shotSpawn.rotation);
		}
	}
}

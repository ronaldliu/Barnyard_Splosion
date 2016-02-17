using UnityEngine;
using System.Collections;


public class WeaponController : Item {
	
	public int ammo;
	// Higher the number the faster the gun shoots
	public float firerate;
	// The Bullet trail
	public GameObject projectile;
	public Transform shotSpawn;
	public float damage;
	public float projectileSpeed;
	Item item;
	// Used for bullet spread so it isnt just straight
	// Higher the number the more variance
	public float variance;

	float nextfire;

	void Awake()
	{
		collider = GetComponent<BoxCollider2D> ();
		shotSpawn = transform.FindChild ("shotspawn");
		item = GetComponent<Item> ();
		if (shotSpawn == null) 
		{
			Debug.LogError ("No shot spawn");
		}
	}

	public void Fire()
	{
		if (Time.time > nextfire && ammo != 0) 
		{
			nextfire += 1 / firerate;
			ammo--;
			Instantiate (projectile, shotSpawn.position, shotSpawn.rotation);
		}
	}
}

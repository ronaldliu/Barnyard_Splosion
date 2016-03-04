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
	// Used for bullet spread so it isnt just straight
	// Higher the number the more variance
	public float variance;

	float nextfire = 0;

	public override void AlignItem(){
		base.AlignItem ();
		Debug.DrawRay(shotSpawn.position,shotSpawn.right * facing,Color.green);
	}
	void Awake()
	{
		collider = GetComponent<BoxCollider2D> ();
		shotSpawn = transform.FindChild ("shotspawn");

		if (shotSpawn == null) 
		{
			Debug.LogError ("No shot spawn");
		}
	}

	public override void Fire()
	{
		if (Time.time > nextfire && ammo != 0) 
		{
			nextfire = Time.time + 1 / firerate;
			ammo--;

			GameObject shot = (GameObject) Instantiate (projectile, shotSpawn.position, shotSpawn.rotation);
			shot.GetComponent<Projectile>().SetupProjectile (collisionMask, fightingMask, this, projectileSpeed, damage,facing, false);
			shot.transform.SetParent (GameObject.Find ("Projectiles").transform);
		}
	}
}

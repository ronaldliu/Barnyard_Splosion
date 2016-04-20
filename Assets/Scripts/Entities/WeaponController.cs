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
	public bool bulletDrop = false;
	// Used for bullet spread so it isnt just straight
	// Higher the number the more variance
	public float variance;
    public AudioSource firingSound;

	float nextfire = 0;

	public override void AlignItem(){
		base.AlignItem ();
		Debug.DrawRay(shotSpawn.position,shotSpawn.right * facing,Color.green);
	}
	void Start()
	{
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
            firingSound.Play();
			GameObject shot = (GameObject)Instantiate (projectile, shotSpawn.position, Quaternion.Euler(Vector3.zero));
			shot.GetComponent<Projectile>().SetupProjectile (collisionMask, fightingMask, this, projectileSpeed, damage,facing,shotSpawn.right, bulletDrop);
			shot.transform.SetParent (GameObject.Find ("Projectiles").transform);
		}
	}
}

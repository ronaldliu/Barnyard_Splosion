using UnityEngine;
using System.Collections;

public class ItemController : RaycastController {

	Player wielder;
	bool grabable = true;
	bool active = false;

	public int ammo;
	public float firerate;
	public GameObject projectile;//Move Weapon specific variable into a weapon script thet extends item!
	public Transform shotSpawn;
	public float pStartX;
	public float pStartY;
	public float damage;
	public float projectileSpeed;

	float nextfire;

	void Awake()
	{
		shotSpawn = transform.FindChild ("ShotSpawn");
		if (shotSpawn == null) 
		{
			Debug.LogError ("No shot spawn");
		}
	}

	void Update ()
	{
		if (firerate == 0) 
		{
			if (Input.GetKeyDown ("Fire_")) 
			{
				Fire ();
			}
		} else {
			if (Input.GetButton ("Fire_") && Time.time > nextfire) 
			{
				nextfire = Time.time + firerate;
				Fire ();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") 
		{
			//wielder = other.gameObject;
			active = true;
			grabable = false;
		}
	}

	void Fire()
	{
		// Instantiate (projectile, shotSpawn.position, shotSpawn.rotation);
	}
}

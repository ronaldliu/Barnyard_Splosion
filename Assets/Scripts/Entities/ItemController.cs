using UnityEngine;
using System.Collections;

public class ItemController : RaycastController {

	Player wielder;
	bool grabable = true;
	bool active = false;

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

	float nextfire;

	void Awake()
	{
		shotSpawn = transform.FindChild ("shotspawn");
		if (shotSpawn == null) 
		{
			Debug.LogError ("No shot spawn");
		}
	}

	void Update ()
	{
		if (firerate == 0) 
		{
			if (Input.GetButtonDown ("Fire_P2") && ammo != 0) 
			{
				Fire ();
				ammo--;
			}
		} else {
			if (Input.GetButton ("Fire_P2") && Time.time > nextfire && ammo != 0) 
			{
				nextfire = Time.time + 1/firerate;
				Fire ();
				ammo--;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") 
		{
			Debug.Log ("I collided with Player");
			active = true;
			grabable = false;
		}
	}

	void Fire()
	{
		// Instantiate (projectile, shotSpawn.position, shotSpawn.rotation);
		// Vector2 mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		Vector2 shotSpawnPos = new Vector2 (shotSpawn.position.x, shotSpawn.position.y);
		Vector2 shotForward = new Vector2 (shotSpawn.right.x, shotSpawn.right.y + Random.Range (-variance/100, variance/100));
		RaycastHit2D hit = Physics2D.Raycast (shotSpawnPos, shotForward, 100, collisionMask);

		Debug.DrawLine (shotSpawnPos, shotForward * 100, Color.cyan);

		if (hit) {
			Player enemy = hit.transform.GetComponent<Player>(); //Create Player Dictionary
			enemy.health -= damage;
		}
	}


}

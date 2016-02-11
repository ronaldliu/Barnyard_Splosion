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
			if (Input.GetButtonDown ("Fire_P1")) 
			{
				Fire ();
			}
		} else {
			if (Input.GetButton ("Fire_P2") && Time.time > nextfire) 
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
			Debug.Log ("I collided with Player");
			active = true;
			grabable = false;
		}
	}

	void Fire()
	{
		// Instantiate (projectile, shotSpawn.position, shotSpawn.rotation);
		Vector2 mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		Vector2 shotSpawnPos = new Vector2 (shotSpawn.position.x, shotSpawn.position.y);
		RaycastHit2D hit = Physics2D.Raycast (shotSpawnPos, mousePos - shotSpawnPos, 100, collisionMask);
		Debug.DrawLine (shotSpawnPos, (mousePos - shotSpawnPos) * 100, Color.green);

		if (hit) {
			Player enemy = hit.transform.GetComponent<Player>(); //Create Player Dictionary
			enemy.health -= damage;
		}
	}
}

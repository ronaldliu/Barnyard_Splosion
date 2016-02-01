using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	Player wielder;
	bool grabable = true;
	bool active = false;

	public int ammo;
	public float firerate;
	public GameObject projectile;
	public float pStartX;
	public float pStartY;
	public float damage;
	public float projectileSpeed;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") 
		{
			wielder = other.gameObject;
			active = true;
			grabable = false;
		}
	}

	void Fire()
	{
		Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + spawnHeight, transform.position.z);
		Instantiate (projectile, spawnPosition, Quaternion.identity);
	}
}

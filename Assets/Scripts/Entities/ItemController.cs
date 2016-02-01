using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	public bool grabable = true;
	public bool active = false;
	public int ammo;
	public float firerate;
	public GameObject projectile;
	public float pStartX;
	public float pStartY;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") 
		{
			// Attach the gun onto player
		}
	}
}

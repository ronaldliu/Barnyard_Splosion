using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

	public GameObject[] weapon;
	public float spawnHeight;
	// Used to choose whether or not to spawn a random Item from list or from choice
	public bool random;
	// Which item to spawn
	public int choice;

	void Start () 
	{
		SpawnWeapon ();
	}

	void SpawnWeapon()
	{
		Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + spawnHeight, transform.position.z);
		if (random)
			Instantiate(weapon[Random.Range(0, weapon.Length)], spawnPosition, Quaternion.identity);
		else
			Instantiate (weapon[choice], spawnPosition, Quaternion.identity);
	}
}
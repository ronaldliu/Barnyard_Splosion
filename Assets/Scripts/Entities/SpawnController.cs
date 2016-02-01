using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

	public GameObject[] weapon;
	public float spawnHeight;
	public int choice;
	// Use this for initialization
	void Start () 
	{
		SpawnWeapon ();
	}

	void SpawnWeapon()
	{
		Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + spawnHeight, transform.position.z);
		Instantiate (weapon[choice], spawnPosition, Quaternion.identity);
	}
}

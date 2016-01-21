using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	private CharacterController characterController;
	public bool isGrounded;
	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		IsGrounded ();
	}
	void IsGrounded () {
		//start pos
		//direction
		//length
		isGrounded = (Physics.Raycast(transform.position, -transfrom.up, characterController.height / 1.8F));

	}
}

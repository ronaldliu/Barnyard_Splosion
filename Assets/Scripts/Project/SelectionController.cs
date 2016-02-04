using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionController : MonoBehaviour {
	public int currentControllerCount = 0;
	// Use this for initialization
	Dictionary<string,ControllerSelector> currentControllerNames = new Dictionary<string,ControllerSelector>();
	GameObject controllers;
	void Start () {
		controllers = GameObject.Find ("Controllers");
		if (Input.GetJoystickNames ().Length != 0) {
			currentControllerCount = Input.GetJoystickNames ().Length;
			foreach (string name in Input.GetJoystickNames()) {

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (currentControllerCount < Input.GetJoystickNames ().Length) {
			foreach (string name in Input.GetJoystickNames ()) {
				if (currentControllerNames.ContainsKey(name)) {
					continue;
				} else {

				}
			}
		}
	}
}

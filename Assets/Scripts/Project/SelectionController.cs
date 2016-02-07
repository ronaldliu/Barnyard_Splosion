using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionController : MonoBehaviour {
	public int controllerCount = 0;
	// Use this for initialization
	Dictionary<string,ControllerSelector> controllerNames = new Dictionary<string,ControllerSelector>();
	GameObject controllers;
	void Start () {
		controllers = GameObject.Find ("Controllers");
		print (Input.GetJoystickNames ().Length);
		if (Input.GetJoystickNames ().Length != 0) {
			controllerCount = Input.GetJoystickNames ().Length;
			string [] name = Input.GetJoystickNames ();
			for(int i = 0; i < Input.GetJoystickNames().Length; i++) {
				ControllerSelector tempRef = controllers.AddComponent<ControllerSelector>();
				tempRef.player = i + 1;
				controllerNames.Add (name[i], tempRef);

			}
		}
	}

	// Update is called once per frame
	void Update () {
		
		List<string> inputNames = new List<string>(Input.GetJoystickNames ());
		if (controllerCount < inputNames.Count) {
			foreach (string name in Input.GetJoystickNames ()) {
				print (name);
				if (controllerNames.ContainsKey (name)) {
					continue;
				} else {
					controllerCount++;
				}
			}
		} else if (controllerCount > inputNames.Count) {
			foreach (string name in controllerNames.Keys) {
				if (inputNames.Contains (name)) {
					continue;
				} else {
					controllerCount--;
					controllerNames.Remove (name);
				}
			}

		} else if (controllerCount == 1 && Input.GetJoystickNames () [0] == "") {
			controllerCount = 0;
		}
	}
}

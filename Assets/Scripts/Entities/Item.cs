using UnityEngine;
using System.Collections;

[RequireComponent (typeof (WeaponController))]
public class Item : MonoBehaviour {
	Vector3 position;
	Vector3 rotation;
	bool held;
	Player holdingMe;

	void Start () {
	
	}
	void Update(){
		if (held) {

		} else {

		}
	}
	public void Fire(){

	}
	public void CatchPlayer(Player me){
		holdingMe = me;
		held = true;
	}
	public void PlayerDrop(){
		holdingMe = null;
		held = false;
	}
}

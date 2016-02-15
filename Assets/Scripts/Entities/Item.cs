using UnityEngine;
using System.Collections;

[RequireComponent (typeof (WeaponController))]
public class Item : RaycastController {
	Vector3 position;
	Vector3 rotation;
	bool held;
	public Player holdingMe;
	WeaponController weapon;

	void Start () {
	
	}
	void Update(){
		if (held) {
			// Get weapon bone position and Always update weapon based on that
			transform.TransformPoint(new Vector3(holdingMe.weap.WorldX, holdingMe.weap.WorldY, 0));
		} else {
			// Gravity, Physics, Things of that nature
		}
	}
	public void Fire(){
		weapon.Fire ();
	}
	public void CatchPlayer(Player me){
		holdingMe = me;
		held = true;
	}
	public void PlayerDrop(){
		holdingMe = null;
		held = false;
	}
	public void ReferenceToItem(WeaponController me)
	{
		weapon = me;
	}
}

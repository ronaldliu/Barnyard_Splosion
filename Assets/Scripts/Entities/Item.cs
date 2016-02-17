using UnityEngine;
using System.Collections;

[RequireComponent (typeof (WeaponController))]
public class Item : RaycastController {
	Vector3 position;
	Vector3 rotation;
	bool held;
	public Player holdingMe;
	WeaponController weapon;
	Vector2 aim;
	void Start () {
		weapon = GetComponent<WeaponController> ();
	}
	void Update(){
		if (held) {
			// Get weapon bone position and Always update weapon based on that
			AlignItem();

			print ("Held");
		} else {
			// Gravity, Physics, Things of that nature
		}
	}
	public void Fire(){
		weapon.Fire ();
	}
	public void CatchPlayer(Player me){
		holdingMe = me;
		transform.SetParent(holdingMe.transform);
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
	void AlignItem(){
		transform.localScale = new Vector3(holdingMe.facing, 1);
		transform.position = new Vector3 ( holdingMe.transform.position.x + holdingMe.weap.WorldX , holdingMe.transform.position.y +holdingMe.weap.WorldY);
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, (holdingMe.facing > 0 )?(holdingMe.arm.rotation+150):360 -(holdingMe.arm.rotation + 150)));
		Debug.DrawRay(weapon.shotSpawn.position,weapon.shotSpawn.right * holdingMe.facing,Color.green);

	}
}

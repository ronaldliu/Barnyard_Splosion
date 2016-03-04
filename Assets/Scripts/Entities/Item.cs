using UnityEngine;
using System.Collections;

public class Item : RaycastController {
	Vector3 position;
	Vector3 rotation;
	public bool held;
	public Player holdingMe;
	public LayerMask fightingMask;
	public int facing = 1;
	Vector2 aim;
	void Start () {
		//print ("Gun");
	}
	void Update(){
		if (held) {
			AlignItem();
		} else {
			//transform.Rotate (new Vector3 (0, 0, 5));

			//print (transform.eulerAngles.z);
			Debug.DrawRay (transform.position,-transform.up - new Vector3 (Mathf.Sin (Mathf.Deg2Rad * transform.eulerAngles.z),Mathf.Cos (Mathf.Deg2Rad * transform.eulerAngles.z)));
			//transform.Translate (new Vector3();
			// Gravity, Physics, Things of that nature
		}
	}
	public virtual void Fire(){
		print ("Fire!");
	}
	public void CatchPlayer(Player me){
		holdingMe = me;
		transform.SetParent(holdingMe.transform);
		fightingMask = me.controller.fightingMask;
		held = true;
	}
	public void PlayerDrop(){
		holdingMe = null;
		held = false;
	}

	public virtual void AlignItem(){
		facing =(int) holdingMe.facing;
		transform.localScale = new Vector3(facing, 1);
		transform.position = new Vector3 ( holdingMe.transform.position.x + holdingMe.weap.WorldX , holdingMe.transform.position.y +holdingMe.weap.WorldY);
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, (holdingMe.facing > 0 ) ? (holdingMe.arm.rotation + 150) : 360 - (holdingMe.arm.rotation + 150)));

	}
}

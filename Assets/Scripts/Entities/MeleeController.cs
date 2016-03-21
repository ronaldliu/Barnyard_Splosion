using UnityEngine;
using System.Collections;

public class MeleeController:Item{
	float lastFire = 0;
	bool swing = false;
	float swingStart = 0;
	void Start () {

	}
	public override void Fire()
	{
		if (Time.time - lastFire > .3f) 
		{
			lastFire = Time.time;
			holdingMe.arm.rotation += 90;

			swing = true;
		}
	}
	// Update is called once per frame
	void LateUpdate () {
		if (swing) {
			if (Time.time - lastFire < .25f) {
				holdingMe.arm.rotation -= 15;
				holdingMe.arms = false;
				base.Fire ();

			} else {
				holdingMe.arms = true;
				swing = false;
			}
		}
	}
	public override void AlignItem(){
		base.AlignItem ();
	}
}

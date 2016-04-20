using UnityEngine;
using System.Collections;

public class MeleeController : Item{
	float lastFire = 0;
	bool swing = false;
	float swingStart = 0;
    public AudioSource swingSound;
	void Start () {
		base.Start ();
	}
	public override void Fire()
	{
		if (Time.time - lastFire > .45f) 
		{
			lastFire = Time.time;
			holdingMe.arm.rotation += 35;

			swing = true;
		}
	}
	// Update is called once per frame
	void LateUpdate () {
		if (swing) {
			if (Time.time - lastFire < .25f) {
				holdingMe.arm.rotation -= 33;
				holdingMe.arms = false;
				Swing ();
				base.Fire ();

			} else {
				holdingMe.arms = true;
				swing = false;
			}
		}
	}
	public void Swing(){
		UpdateItemRaycastOrigins ();
		float directionX = facing;
		float rayLength = skinWidth;
        swingSound.Play();
		for (int i = 0; i <  horizontalRayCount; i++) {
			print (i);
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
		
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right* directionX, rayLength, fightingMask);

			Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if (hit) {
				Player enemy = hit.transform.GetComponent<Player> ();
				enemy.health -= 10;
				enemy.velocity.x = facing * 5;
				break;
			}
		}
	}
	public override void AlignItem(){
		base.AlignItem ();
	}
}

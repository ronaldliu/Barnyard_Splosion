﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Modify to accompany multiple players

//TODO: Make zooming and translations smooth
public class CameraBox : MonoBehaviour {

	public List<Player> targets;
	public Vector2 focusAreaSize;

	public float verticalOffset;
	public float oldBoundScale = 1;

	public Camera cam;
	FocusArea focusArea;

	public void LateStart(){
		cam = GetComponent<Camera>();
		focusArea = new FocusArea (targets,ref focusAreaSize);
	}

	void LateUpdate(){

		foreach (Player t in targets) {
			if (t.IsDead ()) {
				targets.Remove (t);
			}
		}
		focusArea.Update (targets, ref focusAreaSize);

		//Center the Camera First to make calcs easy
		Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;
		transform.position = (Vector3)focusPosition + Vector3.forward * -1;

		if (targets.Capacity > 1) {
			CameraAdjust ();
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = new Color (1, 0, 1, .5f);
		Gizmos.DrawCube (focusArea.center, focusAreaSize);

	}

	//Careful Editing this Section
	//Changing the wrong thing will result in freezing unity
	void CameraAdjust(){
		bool correct = false;
		bool increasing = false;
		bool heightControlled = false;
		while(!correct){
			float width = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane)).x - cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane)).x;
			float height = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane)).y - cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane)).y;
			float heightRatio = (focusAreaSize.y / height);
			float widthRatio = (focusAreaSize.x / width);

			heightControlled = (focusAreaSize.y / height) > (focusAreaSize.x / width);



			if (focusAreaSize.x / width > .9f || focusAreaSize.y/height > .9f && ! increasing){
				//cam.orthographicSize += .1f;
				increasing = true;
				cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize,cam.orthographicSize += .3f,.08f);
				correct = true;
			}else if ((((focusAreaSize.x / width < .65f && focusAreaSize.y/height < .9f)  || (focusAreaSize.y / height < .65f && focusAreaSize.x / width < .9f))) && !increasing) {
				if (width > 10) {
				//	cam.orthographicSize -= .1f;
					cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize,cam.orthographicSize -= .3f,.02f);
					correct = true;
					print("Shrink: " + focusAreaSize.y/height);

				} else {
					print ("Too small");
					correct = true;
				}
			} else {
				correct = true;
			}

		}

	}
	struct FocusArea{
		public Vector2 center;
		public Vector2 velocity;
		float left,right;
		float top,bottom;
		bool lHit,rHit,bHit,tHit;

		public FocusArea(List<Player> players,ref Vector2 focusAreaSize){
			BoxCollider2D player1 = players[0].boxCollider;
			left = player1.bounds.min.x;
			right = player1.bounds.max.x;
			top = player1.bounds.max.y;
			bottom = player1.bounds.min.y;
			foreach (Player current in players){
				BoxCollider2D currentBox = current.boxCollider;
				if(currentBox.bounds.min.x < left){
					left = currentBox.bounds.min.x;
				}
				if(currentBox.bounds.max.x > right){
					right = currentBox.bounds.max.x;
				}
				if(currentBox.bounds.min.y < bottom){
					bottom = currentBox.bounds.min.y;
				}
				if(currentBox.bounds.max.y > top){
					top = currentBox.bounds.max.y;
				}

			}
			focusAreaSize = new Vector2(right - left, top - bottom);
			velocity = Vector2.zero;
			center = new Vector2((left + right) / 2,(top + bottom) / 2);
			lHit = rHit = bHit = tHit = false;
		}
		public void Update(List<Player> players , ref Vector2 focusAreaSize){
			lHit = rHit = bHit = tHit = false;
			foreach (Player current in players){
				BoxCollider2D currentBox = current.boxCollider;
				if ((currentBox.bounds.min.x < left) || !lHit) {
					left = currentBox.bounds.min.x;
					lHit = true;
				}

				if((currentBox.bounds.max.x > right) || !rHit){
					right = currentBox.bounds.max.x;
					rHit = true;
				}

				if((currentBox.bounds.min.y < bottom) || !bHit){
					bottom = currentBox.bounds.min.y;
					bHit = true;
				}

				if((currentBox.bounds.max.y > top) || !tHit){
					top = currentBox.bounds.max.y;
					tHit = true;
				}
			}
			focusAreaSize = new Vector2(right - left, top - bottom);
			velocity = Vector2.zero;
			center = new Vector2((left + right) / 2,(top + bottom) / 2);
		}
	}
}

using UnityEngine;
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
		List<Player> tempTargets = new List<Player>();
		//Below is the fix for modifying a list while accessing it error that will happen otherwise
		foreach (Player t in targets) {
			if (t.IsDead ()) {
				tempTargets.Add(t);
			}
		}
		foreach (Player r in tempTargets) {
			targets.Remove (r);
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
		//Gizmos.DrawCube (focusArea.center, focusAreaSize);

	}

	//Careful Editing this Section
	//Changing the wrong thing will result in freezing unity
	void CameraAdjust(){
		bool increasing = false;
		bool heightControlled = false;

		float width = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane)).x - cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane)).x;
		float height = cam.ViewportToWorldPoint(new Vector3(1,1,cam.nearClipPlane)).y - cam.ViewportToWorldPoint(new Vector3(0,0,cam.nearClipPlane)).y;
		float heightRatio = (focusAreaSize.y / height);
		float widthRatio = (focusAreaSize.x / width);

		heightControlled = heightRatio > widthRatio;
		if (heightControlled) {
			if (focusAreaSize.y / height > .85f) {
				increasing = true;
				cam.orthographicSize += .03f;
			} else if ((focusAreaSize.y / height < .65f) && !increasing) {
				if (width > 17) {
					cam.orthographicSize -= .03f;
				}
			} 
		} else {
			if (focusAreaSize.x / width > .85f) {
				increasing = true;
				cam.orthographicSize += .03f;
			} else if ((focusAreaSize.x / width < .65f) && !increasing) {
				if (width > 17) {
					cam.orthographicSize -= .03f;
				}
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

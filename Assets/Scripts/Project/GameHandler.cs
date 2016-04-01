using UnityEngine;
using System.Collections;

public class GameHandler: MonoBehaviour {
	public GameObject yes;
	public CameraBox cam;
	public float timeLimit;
	public Font myfont = new Font();
	// Use this for initialization
	void Start () {
		timeLimit *= 60;
		//yes = GameObject.Find ("GameOptions");
		//var go = Instantiate(yes.GetComponent<GameOptions> ().p1, transform.position, transform.rotation); //How to add players
		//cam.targets.Add(yes.GetComponent<GameOptions> ().p1.GetComponent<Player>());
	}
	
	void Update () {
		if (timeLimit - Time.time <= 0) {
			Time.timeScale = 0;
		}
	}
	void OnGUI(){
		GUI.skin.font = myfont;
		GUI.contentColor = Color.black;
		if (timeLimit - Time.time >= 0) {
			GUI.Label (new Rect (300, 10, 200, 1000), "" + (Mathf.Floor ((timeLimit / 60) - Time.time / 60)) + ":" + (Mathf.Round (Time.time % 60) > 50 ? "0" : "") + (60 - Mathf.Round (Time.time % 60)));
		} else {

		}
	}
	void InitiateGameOver(){


	}
}

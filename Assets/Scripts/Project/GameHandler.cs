using UnityEngine;
using System.Collections;

public class GameHandler: MonoBehaviour {
	public GameObject yes;
	public CameraBox cam;
	public float timeLimit = 5;
	public Font myfont;
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
			GameObject.Find("Time").GetComponent<UnityEngine.UI.Text>().text = "" + (Mathf.Floor ((timeLimit / 60) - Time.time / 60)) + ":" + (Mathf.Round (Time.time % 60) > 50 ? "0" : "") + (60 - Mathf.Round (Time.time % 60));
		} else {
			GUI.Label (new Rect (300, 10, 200, 1000), "00:00");
			GUI.Label (new Rect (300,500, 200, 1000), "Match End");
			//new WaitForSeconds(10);
			InitiateGameOver ();
		}
	}
	void InitiateGameOver(){
		GameObject.Find ("GameState").GetComponent<UnityEngine.UI.Text> ().text = "Match End";

	}
}

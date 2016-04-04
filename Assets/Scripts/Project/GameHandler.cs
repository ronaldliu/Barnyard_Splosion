using UnityEngine;
using System.Collections;

public class GameHandler: MonoBehaviour {
	public GameObject yes;
	public CameraBox cam;
	public float timeLimit = 5;
	public bool gameEnd = false;
	float lastTime;

	// Use this for initialization
	void Start () {
		//timeLimit = 5;
		timeLimit *= 60;
		Time.timeScale = 1;
		gameEnd = false;
		//yes = GameObject.Find ("GameOptions");
		//var go = Instantiate(yes.GetComponent<GameOptions> ().p1, transform.position, transform.rotation); //How to add players
		//cam.targets.Add(yes.GetComponent<GameOptions> ().p1.GetComponent<Player>());
	}
	
	void Update () {
		if (timeLimit - Time.time <= 0) {
			//Time.timeScale = 0;
		}
	}
	void OnGUI(){
		GUI.contentColor = Color.black;
		if (timeLimit - Time.time >= 0) {
			GameObject.Find ("Time").GetComponent<UnityEngine.UI.Text> ().text = "" + (Mathf.Floor ((timeLimit / 60) - Time.time / 60)) + ":" + (Mathf.Round (Time.time % 60) > 50 ? "0" : "") + (60 - Mathf.Round (Time.time % 60));
			lastTime = Time.time;
		} else {
			InitiateGameOver ();
		}
	}
	void InitiateGameOver(){
		gameEnd = true;
		GameObject.Find ("GameState").GetComponent<UnityEngine.UI.Text> ().text = "Match End";
		StartCoroutine(MenuDelay ());

	}
	IEnumerator MenuDelay(){
		yield return new WaitForSeconds (5);
		LevelManager reset = new LevelManager ();
		reset.LoadScene ("MainMenu");
	}
}

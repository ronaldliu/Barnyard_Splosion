using UnityEngine;
using System.Collections;

public class GameHandler: MonoBehaviour {
	public GameObject yes;
	public CameraBox cam;
	public float timeLimit = 5;
	private const float timelim = 2;
	public float timeStart;
	public UnityEngine.UI.Text timer;
	public bool gameEnd;

	// Use this for initialization
	void Start () {
		timeLimit = timelim;
		timeLimit *= 60;
		gameEnd = true;
		timer = GameObject.Find ("Time").GetComponent<UnityEngine.UI.Text> ();
		StartCoroutine(StartDelay ());

		//yes = GameObject.Find ("GameOptions");
		//var go = Instantiate(yes.GetComponent<GameOptions> ().p1, transform.position, transform.rotation); //How to add players
		//cam.targets.Add(yes.GetComponent<GameOptions> ().p1.GetComponent<Player>());
	}

	//This will be used to determine stats about the current game round such as kills, people who are out and whether to end the round
	void Update () {
		if (timeLimit - (Time.time - timeStart) <= 0) {
			//Time.timeScale = 0;
		}
	}
	void OnGUI(){
	//This method is launched right before drawing Graphics, so it is used to update stats such as lives, kills, health, and time
		if (!gameEnd) {
			if (timeLimit - (Time.time - timeStart) >= 0) {
				timer.text = "" + (Mathf.Floor ((timeLimit / 60) - Mathf.Ceil ((Time.time - timeStart) / 60))) + ":";
				timer.text += (Mathf.Floor ((Time.time - timeStart) % 60) > 49 ? "0" : "") + (59 - Mathf.Floor ((Time.time - timeStart) % 60));

			} else {
				InitiateGameOver ();
			}
		}
	}
	//This is used to End 
	void InitiateGameOver(){
		gameEnd = true;
		GameObject.Find ("GameState").GetComponent<UnityEngine.UI.Text> ().text = "Match End";
		StartCoroutine(MenuDelay ());

	}
	IEnumerator MenuDelay(){
		yield return new WaitForSeconds (5);
		LevelManager reset = transform.gameObject.AddComponent<LevelManager>();
		Time.timeScale = 0;
		reset.LoadScene ("MainMenu");
	}
	IEnumerator StartDelay(){
		timer.text = timeLimit / 60 + ":00";
		Time.timeScale = 1;
		yield return new WaitForSeconds (3);
		timeStart = Time.time;
		gameEnd = false;
	}
}

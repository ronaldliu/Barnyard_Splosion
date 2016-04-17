using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHandler: MonoBehaviour {
	public GameObject options;
	public CameraBox cam;
	public float timeLimit = 5;
	private const float timelim = 2;
	public float timeStart;
	public UnityEngine.UI.Text timer;
	public bool gameEnd;
	GameOptions info;
	public GameObject[] bars;
	public Transform [] playerStarts;
	// Use this for initialization
	void Awake () {
		Time.timeScale = 0;
		timeLimit = timelim;
		timeLimit *= 60;
		gameEnd = true;
		timer = GameObject.Find ("Time").GetComponent<UnityEngine.UI.Text> ();

		options = GameObject.Find ("GameOptions");
		info = options.GetComponent<GameOptions> ();

		InstantiatePlayers (1);
		StartCoroutine(StartDelay ());





		//var go = Instantiate(yes.GetComponent<GameOptions> ().p1, transform.position, transform.rotation); //How to add players
		//cam.targets.Add(yes.GetComponent<GameOptions> ().p1.GetComponent<Player>());
	}
	void InstantiatePlayers(int numPlayers){
		Player[] players = {
			info.p1.GetComponent<Player> (),
			info.p2.GetComponent<Player> (),
			info.p3.GetComponent<Player> (),
			info.p4.GetComponent<Player> ()
		};

		GameObject player1 =(GameObject) Instantiate (players[0].gameObject,playerStarts[0].position,playerStarts[0].rotation);
		GameObject player2 =(GameObject) Instantiate (players[1].gameObject,playerStarts[1].position,playerStarts[1].rotation);
		GameObject player3 =(GameObject) Instantiate (players[2].gameObject,playerStarts[2].position,playerStarts[2].rotation);
		GameObject player4 =(GameObject) Instantiate (players[3].gameObject,playerStarts[3].position,playerStarts[3].rotation);

		players = new Player[]{
			player1.GetComponent<Player>(),
			player2.GetComponent<Player>(),
			player3.GetComponent<Player>(),
			player4.GetComponent<Player>()
		};
		players[0].player = "P1";
		players[1].player = "P2";

		players[2].player = "P3";
		players[3].player = "P4";

		players [0].healthbar = bars [0];
		players[1].healthbar = bars [1];
		players[2].healthbar = bars [2];
		players[3].healthbar = bars [3];

		players[0].gameObject.layer = LayerMask.NameToLayer("Player 1");
		players[1].gameObject.layer = LayerMask.NameToLayer ("Player 2");
		players[2].gameObject.layer =  LayerMask.NameToLayer("Player 3");
		players[3].gameObject.layer =  LayerMask.NameToLayer("Player 4");

		players[0].boxCollider = players[0].transform.GetComponent<BoxCollider2D> ();
		players[1].boxCollider = players[1].transform.GetComponent<BoxCollider2D> ();
		players[2].boxCollider = players[2].transform.GetComponent<BoxCollider2D> ();		
		players[3].boxCollider = players[3].transform.GetComponent<BoxCollider2D> ();

		cam.targets.Add (players [0]);
		cam.targets.Add(players[1]);
		cam.targets.Add(players[2]);
		cam.targets.Add(players[3]);
		/*
		players [0].controller.fightingMask = LayerMask.NameToLayer ("Player 2") + LayerMask.NameToLayer ("Player 3") + LayerMask.NameToLayer ("Player 4");
		players [1].controller.fightingMask = LayerMask.NameToLayer ("Player 1") + LayerMask.NameToLayer ("Player 3") + LayerMask.NameToLayer ("Player 4");
		players [2].controller.fightingMask = LayerMask.NameToLayer ("Player 2") + LayerMask.NameToLayer ("Player 1") + LayerMask.NameToLayer ("Player 4");
		players [3].controller.fightingMask = LayerMask.NameToLayer ("Player 2") + LayerMask.NameToLayer ("Player 3") + LayerMask.NameToLayer ("Player 1");
*/
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
	//This is used to End the current round
	void InitiateGameOver(){
		gameEnd = true;
		GameObject.Find ("GameState").GetComponent<UnityEngine.UI.Text> ().text = "Match End";
		StartCoroutine(MenuDelay ());

	}

	//This delay is to denote the end of the round
	IEnumerator MenuDelay(){
		yield return new WaitForSeconds (5);
		LevelManager reset = transform.gameObject.AddComponent<LevelManager>();
		Time.timeScale = 0;
		reset.LoadScene ("MainMenu");
	}

	//This is used to start the round after a period of preperation
	IEnumerator StartDelay(){
		timer.text = timeLimit / 60 + ":00";
		Time.timeScale = 1;
		yield return new WaitForSeconds (3);
		timeStart = Time.time;
		gameEnd = false;
	}
}

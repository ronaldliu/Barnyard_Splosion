using UnityEngine;
using System.Collections;

public class GameHandler: MonoBehaviour {
	public GameObject yes;
	public CameraBox cam;
	public float timeLimit;
	// Use this for initialization
	void Start () {
		timeLimit = 20 * 60;
		//yes = GameObject.Find ("GameOptions");
		//var go = Instantiate(yes.GetComponent<GameOptions> ().p1, transform.position, transform.rotation); //How to add players
		//cam.targets.Add(yes.GetComponent<GameOptions> ().p1.GetComponent<Player>());
	}
	
	void Update () {
		
	}
	void OnGUI(){
		GUI.Label(new Rect(350,20,200,1000),"" + (Mathf.Round(20 - Time.time/60))+":"+(Mathf.Round(Time.time % 60) >50?"0":"")+ (60 - Mathf.Round(Time.time % 60)));
	}
}

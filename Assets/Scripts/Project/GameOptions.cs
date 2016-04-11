using UnityEngine;
using System.Collections;

public class GameOptions : MonoBehaviour {
	public string level;
	public GameObject p1, p2, p3, p4;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

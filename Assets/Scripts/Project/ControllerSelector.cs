using UnityEngine;
using System.Collections;

public class ControllerSelector : MonoBehaviour {

	public int player;
	UnityEngine.UI.Image cursor;
	public Sprite job;
	void Start () {
		cursor = gameObject.AddComponent<UnityEngine.UI.Image> ();
		cursor.sprite = Resources.Load ("TempCursor", typeof(Sprite)) as Sprite;
	}

	
	// Update is called once per frame
	void Update () {
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal_P" + player), Input.GetAxisRaw ("Vertical_P" + player));
		cursor.transform.Translate (input);
	}
}

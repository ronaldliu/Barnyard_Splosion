using UnityEngine;
using System.Collections;

public class MapMenu : MonoBehaviour {
	public Selectable[] maps;
	public int current;
	int numControllers;
	float limiter = 0;

	void Start()
	{
		string[] joysticks = Input.GetJoystickNames ();
		numControllers = joysticks.Length;
	}

	void Update()
	{
		for (int i = 0; i < numControllers; i++) 
		{
			if (Time.time > limiter && Input.GetAxisRaw ("Horizontal_P" + (i + 1)) > 0) {
				limiter = Time.time + 1 / 20;
				if (current + 1 > maps.Length - 1)
					current = 0;
				else
					current++;
			} else if (Time.time > limiter && Input.GetAxisRaw ("Horizontal_P" + (i + 1)) < 0) {
				limiter = Time.time + 1 / 20;
				if (current - 1 < 0)
					current = maps.Length - 1;
				else
					current--;
			}

			if (Input.GetButtonDown ("Jump_P" + (i + 1)))
				maps [current].selectCharacter ();
		}
	}
}

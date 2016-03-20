using UnityEngine;
using System.Collections;

public class CharacterMenu : MonoBehaviour {
	public Selectable[] characters;
	public int current;
	public int[] selected;
	bool[] accepted;
	int numControllers;
	float limiter = 0;

	void Start()
	{
		string[] joysticks = Input.GetJoystickNames ();
		selected = new int[joysticks.Length];
		numControllers = joysticks.Length;
	}

	void Update()
	{
		for (int i = 0; i < numControllers; i++) 
		{
			if (Time.time > limiter && Input.GetAxisRaw ("Horizontal_P" + (i + 1)) > 0) {
				limiter = Time.time + 1 / 20;
				if (current + 1 > characters.Length - 1)
					current = 0;
				else
					current++;
			} else if (Time.time > limiter && Input.GetAxisRaw ("Horizontal_P" + (i + 1)) < 0) {
				limiter = Time.time + 1 / 20;
				if (current - 1 < 0)
					current = characters.Length - 1;
				else
					current--;
			}

			selected [i] = current;

			if (Input.GetButtonDown ("Jump_P" + (i + 1))) {
				accepted [i] = true;
			}
		}

		bool allAccept = false;
		foreach (bool ans in accepted) {
			if (ans == false)
				break;
		}

		if (allAccept) {
			// Display a ready screen to move to map select menu
		}
	}
}

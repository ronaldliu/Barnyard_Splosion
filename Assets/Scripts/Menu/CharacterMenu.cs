using UnityEngine;
using System.Collections;

public class CharacterMenu : MonoBehaviour {
	public Selectable[] characters;
	public int current;
	public int[] selected;
	bool[] accepted;
	int numControllers;
	float limiter = 0;
	bool canInteract = true;

	void Start()
	{
		string[] joysticks = Input.GetJoystickNames ();
		selected = new int[joysticks.Length];
		numControllers = joysticks.Length;
		accepted = new bool[] { false, false, false };
	}

	void Update()
	{
		int input;
		for (int i = 0; i < numControllers; i++) {
			input = (int) Input.GetAxisRaw ("Horizontal_P" + (i + 1));
			if (input != 0 && canInteract) {
				canInteract = false;
				StartCoroutine (SelectionChange (input));
			}
		}
	}

	IEnumerator SelectionChange(int input)
	{
		if (input > 0 && current < characters.Length - 1) {
			current++;
		} else if (input < 0 && current > 0) {
			current--;
		}
		yield return new WaitForSeconds (0.2f);
		canInteract = true;
	}
}

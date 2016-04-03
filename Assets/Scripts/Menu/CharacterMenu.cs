using UnityEngine;
using System.Collections;

public class CharacterMenu : MonoBehaviour {
	public Selectable[] characters;
	public int current;
	public int[] selected;
	bool[] canInteract;
	int numControllers;
	float limiter = 0;

	void Start()
	{
		string[] joysticks = Input.GetJoystickNames ();
		selected = new int[joysticks.Length];
		numControllers = joysticks.Length;
		canInteract = new bool[] { true, true, true };
		selected = new int[] { 0, 0, 0 };
	}

	void Update()
	{
		int input;
		for (int i = 0; i < 3; i++) {
			input = (int) Input.GetAxisRaw ("Horizontal_P" + (i + 1));
			if (input != 0 && canInteract[i]) {
				canInteract[i] = false;
				StartCoroutine (SelectionChange (input, i));
			}
		}
	}

	IEnumerator SelectionChange(int input, int controller)
	{
		if (input > 0 && selected[controller] < characters.Length - 1) {
			selected[controller]++;
		} else if (input < 0 && selected[controller] > 0) {
			selected[controller]--;
		}
		yield return new WaitForSeconds (0.2f);
		canInteract[controller] = true;
	}
}

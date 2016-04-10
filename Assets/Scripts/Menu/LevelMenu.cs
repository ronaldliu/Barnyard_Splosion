using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour {
	public int current;
	public string[] maps;
	public Selectable[] pmaps;
	bool[] canInteract;
	bool readyToLoad = false;
	int numControllers;
	float limiter = 0;

	void Start()
	{
		string[] joysticks = Input.GetJoystickNames ();
		current = 0;
		numControllers = joysticks.Length;
		canInteract = new bool[] { true, true, true };
		pmaps [current].GetComponent<SpriteRenderer> ().color = Color.magenta;
	}

	void Update()
	{
		int input;
		for (int i = 0; i < 3; i++) {
			input = (int) Input.GetAxisRaw ("Vertical_P" + (i + 1));
			if (input != 0 && canInteract[i]) {
				canInteract[i] = false;
				StartCoroutine (SelectionChange (input, i));
			}
			if (Input.GetButtonDown ("Accept_P" + (i + 1))) {
				if (readyToLoad == true)
					SceneManager.LoadScene (maps[current]);
				else
					readyToLoad = true;
			}
		}
	}

	float getSubAmount(int ctrl)
	{
		if (ctrl == 0)
			return 1;
		else if (ctrl == 1)
			return 0;
		else
			return -1;
	}

	IEnumerator SelectionChange(int input, int controller)
	{
		pmaps [current].GetComponent<SpriteRenderer> ().color = Color.white;

		if (input < 0 && current < maps.Length - 1) {
			current++;
		} else if (input > 0 && current > 0) {
			current--;
		}

		float subFrom = getSubAmount (controller);
		readyToLoad = false;

		pmaps [current].GetComponent<SpriteRenderer> ().color = Color.magenta;

		yield return new WaitForSeconds (0.2f);
		canInteract[controller] = true;
	}
}

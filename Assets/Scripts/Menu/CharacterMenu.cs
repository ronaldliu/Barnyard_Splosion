using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterMenu : MonoBehaviour {
	public int current;
	public Selectable[] characters;
	public Sprite[]
	public int[] selected;
	bool[] canInteract;
	bool loadNextScene;
	int numControllers;
	float limiter = 0;

	void Start()
	{
		string[] joysticks = Input.GetJoystickNames ();
		selected = new int[joysticks.Length];
		numControllers = joysticks.Length;
		canInteract = new bool[] { true, true, true };
		selected = new int[] { 0, 0, 0 };
		loadNextScene = false;
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
			if (Input.GetButtonDown("Accept_P" + (i+1)))
			{
				if (loadNextScene == true) {
					SceneManager.LoadScene ("LevelSelect");
				}
				else
					loadNextScene = true;
			}
		}
	}

	void UpdateCharacters(int curCtrl)
	{
		
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
		if (input > 0 && selected[controller] < characters.Length - 1) {
			selected[controller]++;
		} else if (input < 0 && selected[controller] > 0) {
			selected[controller]--;
		}

		UpdateCharacters ();
		yield return new WaitForSeconds (0.2f);
		canInteract[controller] = true;
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterMenu : MonoBehaviour {
	public int current;
	public GameObject[] characters;
	public GameObject[] availCharacters;
	public Sprite[] characterSprite;
	public int[] selected;

	GameObject gameOptions;
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
		selected = new int[] { 0, 0, 0, 0 };
		gameOptions = GameObject.Find ("GameOptions");
		loadNextScene = false;
		if (numControllers < 4)
			characters [3].GetComponent<SpriteRenderer> ().color = Color.clear;
	}

	void Update()
	{
		float input;
		for (int i = 0; i < 3; i++) {
			input = Input.GetAxisRaw ("Horizontal_P" + (i + 1));
			if (input != 0 && canInteract[i]) {
				canInteract[i] = false;
				StartCoroutine (SelectionChange (input, i));
			}
			if (Input.GetButtonDown("Accept_P" + (i+1)))
			{
				if (loadNextScene == true) {
					gameOptions.GetComponent<GameOptions> ().p1 = availCharacters[selected[0]];
					gameOptions.GetComponent<GameOptions> ().p2 = availCharacters[selected[1]];
					gameOptions.GetComponent<GameOptions> ().p3 = availCharacters[selected[2]];
					gameOptions.GetComponent<GameOptions> ().p4 = availCharacters[selected[3]];
					SceneManager.LoadScene ("LevelSelect");
				}
				else
					loadNextScene = true;
			}
		}
	}

	void UpdateCharacters()
	{
		for (int i = 0; i < 3; i++) 
		{
			characters [i].GetComponent<SpriteRenderer> ().sprite = characterSprite [selected [i]];
		}
	}

	IEnumerator SelectionChange(float input, int controller)
	{
		loadNextScene = false;

		if (input > 0 && selected[controller] < availCharacters.Length - 1) {
			selected[controller]++;
		} else if (input < 0 && selected[controller] > 0) {
			selected[controller]--;
		}

		UpdateCharacters ();
		yield return new WaitForSeconds (0.2f);
		canInteract[controller] = true;
	}
}

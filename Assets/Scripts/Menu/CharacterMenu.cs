using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterMenu : MonoBehaviour {
	public int current;
	public GameObject[] characters;
	public GameObject[] CharBG;
	public GameObject[] availCharacters;
	public Sprite[] characterSprite;
	public Sprite[] characterBackground;
	public int[] selected;
	public int numControllers;

	GameObject gameOptions;
	GameObject PressA;
	bool[] canInteract;
	bool loadNextScene;
	float limiter = 0;

	void Start()
	{
		string[] joysticks = Input.GetJoystickNames ();
		selected = new int[joysticks.Length];
		canInteract = new bool[] { true, true, true };
		selected = new int[] { 0, 0, 0, 0 };
		gameOptions = GameObject.Find ("GameOptions");
		PressA = GameObject.Find ("PressAtoPlay");
		PressA.GetComponent<SpriteRenderer> ().color = Color.clear;
		loadNextScene = false;
	}

	void Update()
	{
		float input;
		for (int i = 0; i < numControllers; i++) {
			input = Input.GetAxisRaw ("Horizontal_P" + (i + 1));
			if (input != 0 && canInteract[i]) {
				canInteract[i] = false;
				StartCoroutine (SelectionChange (input, i));
			}
			if (Input.GetButtonDown("Accept_P" + (i+1)))
			{
				if (loadNextScene == true) {
					gameOptions.GetComponent<GameOptions> ().p1 = availCharacters [selected [0]];
					gameOptions.GetComponent<GameOptions> ().p2 = availCharacters [selected [1]];
					gameOptions.GetComponent<GameOptions> ().p3 = availCharacters [selected [2]];
					gameOptions.GetComponent<GameOptions> ().p4 = availCharacters [selected [3]];
					SceneManager.LoadScene ("LevelSelect");
				} else {
					PressA.GetComponent<SpriteRenderer> ().color = Color.white;
					loadNextScene = true;
				}
			}
		}
	}

	void UpdateCharacters()
	{
		for (int i = 0; i < numControllers; i++) 
		{
			characters [i].GetComponent<SpriteRenderer> ().sprite = characterSprite [selected [i]];
			CharBG [i].GetComponent<SpriteRenderer> ().sprite = characterBackground [selected [i]];
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

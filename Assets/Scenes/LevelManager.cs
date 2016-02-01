using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public Transform mainMenu, optionsMenu;
	public UnityEngine.EventSystems.EventSystem events;
	// Use this for initialization

	public void LoadScene(string name){
		Application.LoadLevel(name);
	}
	public void QuitGame(){
		Application.Quit();
	}
	public void OptionsMenu(bool clicked){
		if (clicked == true) {
			optionsMenu.gameObject.SetActive (clicked);
			mainMenu.gameObject.SetActive (false);
			events.SetSelectedGameObject (optionsMenu.FindChild ("Controls").gameObject);

		} else {
			optionsMenu.gameObject.SetActive (clicked);
			mainMenu.gameObject.SetActive (true);
			events.SetSelectedGameObject (mainMenu.FindChild ("StartGame").gameObject);
		}
	}


}

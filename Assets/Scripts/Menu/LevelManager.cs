using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour {
	public Transform mainMenu, optionsMenu;
	public UnityEngine.EventSystems.EventSystem events;

	void Start(){
		print ("Menu");
		Time.timeScale = 0;
	}
	public void LoadScene(string name){
		SceneManager.LoadScene(name);
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

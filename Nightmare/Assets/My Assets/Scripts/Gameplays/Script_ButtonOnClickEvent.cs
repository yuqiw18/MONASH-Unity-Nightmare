using UnityEngine;
using System.Collections;

public class Script_ButtonOnClickEvent : MonoBehaviour {

	// Event for loading level
	public void loadScene(string sceneName){
		StartCoroutine (WaitAndLoadScene (sceneName));
	}

	// Event for quiting game
	public void quitGame(){
		Application.Quit();
	}

	// Wait for 2secs and load level to make it smoother
	IEnumerator WaitAndLoadScene(string sceneName){
		yield return new WaitForSeconds (2.0f);
		Application.LoadLevel (sceneName);

	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Script_FadeInOut : MonoBehaviour
{
	public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
	public bool fadeIn;
	private float alphaValue;


	void Start(){

		fadeIn = false;
		alphaValue = 1;

	}


	void Update(){

		if (alphaValue < 0) {
			alphaValue = 0;
			TurnOnOff();
		}

		if (alphaValue > 1) {
			alphaValue = 1;
			TurnOnOff();
		}

		// Fade in
		if (fadeIn && alphaValue < 1) {
			gameObject.GetComponent<Image> ().color = new Color (0, 0, 0, alphaValue);
					
			alphaValue += Time.deltaTime * fadeSpeed;
		} 


		// Fade out
		if (!fadeIn && alphaValue > 0) {
					
			gameObject.GetComponent<Image> ().color = new Color (0, 0, 0, alphaValue);
					
			alphaValue -= Time.deltaTime * fadeSpeed;
		}
	}


	public void TurnOnOff(){

		if (gameObject.GetComponent<Image> ().enabled == true) {
		
			gameObject.GetComponent<Image> ().enabled = false;

		} else {
		
			gameObject.GetComponent<Image> ().enabled = true;

		}
	}

	public void Fade(){
		fadeIn = !fadeIn;
	}
	
}

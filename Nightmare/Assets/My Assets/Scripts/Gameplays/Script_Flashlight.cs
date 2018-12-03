using UnityEngine;
using System.Collections;

public class Script_Flashlight : MonoBehaviour {
	
	public AudioClip toggle; // Sound
	private bool flashlightOn; // Switch
	
	// Use this for initialization
	void Start () {
	
		flashlightOn = false;
		GetComponent<Light>().enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.F)) {

			if (flashlightOn){

				GetComponent<Light>().enabled = false;
				flashlightOn = false;

			}else{

				flashlightOn = true;
				GetComponent<Light>().enabled = true;
			
			}
		
			AudioSource.PlayClipAtPoint(toggle,transform.position);// Play the sound when toggle on/off the flashlight
		
		}
	}
}

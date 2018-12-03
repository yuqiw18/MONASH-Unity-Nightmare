using UnityEngine;
using System.Collections;

public class Script_PlayerMovement : MonoBehaviour {

	#region Declare Variables

	public float walkSpeed;
	public float runSpeed;
	public float stamina;
	public AudioClip runSound;
	public AudioClip breathing;

	private GameObject playerObj;
	private GameObject GUIObj;
	private GameObject soundObj;
	private float wait;
	private bool canRun;

	#endregion
	
	// Use this for initialization
	void Start () {
	
		wait = 0;
		stamina = 100;
		canRun = true;
		soundObj = GameObject.Find("Heavy_Breathing");

	}
	
	// Update is called once per frame
	void Update () {

		if (stamina > 100) {
			stamina = 100;
		}

		if (stamina < 0) {
			stamina = 0;
		}

		// Heavy breathing
		if (stamina <= 20) {

			soundObj.gameObject.GetComponent<AudioSource> ().mute = false;
		} else {
		
			soundObj.gameObject.GetComponent<AudioSource> ().mute = true;
		}

		if (Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.W) && stamina > 0) {

			/*
			if (Time.time > wait) {

				wait = Time.time + 0.3f;

				AudioSource.PlayClipAtPoint(runSound,transform.position);

			}
			*/

			// Player can run
			gameObject.GetComponent<CharacterMotorC> ().movement.maxForwardSpeed = runSpeed;
			stamina -= 5.0f * Time.deltaTime;

		} else {
		
			// Player is tired and the movement speed is even slower than walking
			gameObject.GetComponent<CharacterMotorC> ().movement.maxForwardSpeed = walkSpeed;
			stamina += 2.5f * Time.deltaTime;
		
		}	

	}
	
	void OnGUI(){

		GUIObj = GameObject.Find("GUI_StaminaBar");
		GUIObj.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(stamina,10);

	}


	void OnCollisionEnter(Collision theObject){

		if (theObject.gameObject.tag == "Floor") {
			canRun = true;
		} else {
			canRun = false;
		}
	}

}

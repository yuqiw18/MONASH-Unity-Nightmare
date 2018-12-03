using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Script_PlayerStatus : MonoBehaviour {

	#region Declare Variables

	public float health = 100;
	public AudioClip[] painSounds;

	private GameObject GUIObj;
	private GameObject FPSCon;
	public bool losingHealth;
	private float losingHealthTimer;
	
	#endregion

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		healthMod();

		if (losingHealth) {

			health -= 2.0f *Time.deltaTime;

			LoseHealth();
		}

		// Player will keep losing health for 3 secs
		/*
		if (losingHealth && Time.time > losingHealthTimer) {
		
			losingHealthTimer = Time.time + 5.0f;

			losingHealth = false;

		}
		*/
	}


	void healthMod(){

		//Avoid health overflows
		if (health > 100){
			health = 100;
		}

		//Recovery based on time
		if (health >= 80 && health < 100) {
			health += Time.deltaTime * 0.15f;

		}

		//Aggravation based on time
		if (health > 0 && health < 15) {
			health -= Time.deltaTime * 0.075f;
		}

		//Game Over
		if (health <= 0) {
			health = 0;
			ChangeLevel("End");
		}

	}

	void OnTriggerEnter(Collider theObject){
		if (theObject.gameObject.tag == "Blast" && theObject != null) {
			health -= (25 + Random.Range (-15, 16));
			Physics.IgnoreCollision(theObject.GetComponent<Collider>(), GetComponent<Collider>());
		}
	}
	
	public void PlaySound(){
	
		AudioSource.PlayClipAtPoint (painSounds [Random.Range(0,painSounds.Length)], transform.position);
	
	}

	
	void OnGUI(){
		int showHealth = (int)health;
		GUIObj = GameObject.Find("GUI_HealthBar");
		GUIObj.gameObject.GetComponent<Text> ().text = showHealth.ToString ();
	}


	public void SetHealthStatus(bool status){

		losingHealth = status;

	}


	public void ChangeLevel(string levelName){

		// Screen turns black
		GUIObj = GameObject.Find("GUI_FadeInOut");
		GUIObj.gameObject.GetComponent<Image> ().enabled = true;
		GUIObj.gameObject.GetComponent<Script_FadeInOut> ().fadeIn = true;

		// Disable FPS Controller
		FPSCon = GameObject.Find("First Person Controller");
		FPSCon.gameObject.GetComponent<MouseLook> ().enabled = false;
		FPSCon.gameObject.GetComponent<FPSInputControllerC> ().enabled = false;
		FPSCon.gameObject.GetComponent<CharacterController> ().enabled = false;

		// Change level
		StartCoroutine (WaitAndLoad (levelName));
	}

	IEnumerator WaitAndLoad(string levelName){

		yield return new WaitForSeconds (2.0f);
		Application.LoadLevel (levelName);

	}


	void LoseHealth(){

		StartCoroutine (WaitAndRecover());
	}


	IEnumerator WaitAndRecover(){
		yield return new WaitForSeconds (3.0f);
		losingHealth = false;
	}

}

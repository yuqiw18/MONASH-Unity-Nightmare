using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Script_GarageDoor : MonoBehaviour {

	public float speed;
	private float startYPos;
	public float distance;
	public bool on; // Switch

	// Use this for initialization
	void Start () {
	
		on = false;
		startYPos = gameObject.transform.position.y;

	}
	
	// Update is called once per frame
	void Update () {

		if (on) { // If turn on

			// Open or close the garage door at the certain height
			if (gameObject.transform.position.y > startYPos - distance) {
				
				gameObject.transform.position -= new Vector3 (0, Time.deltaTime * speed, 0);
			}else{

				on = false; // Turn off

			}
		}

	}

	// Zombie can't go through the door, they will stop at the door
	void OnCollisionEnter(Collision theObject){

		if (theObject.gameObject.tag == "Zombie") {
		
			theObject.gameObject.GetComponent<Script_Zombie>().moveSpeed = 0;
		
		}
	}
}

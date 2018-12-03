using UnityEngine;
using System.Collections;

public class Script_PhysicalProp : MonoBehaviour {


	#region Declare variables

	public float moveSpeed;
	public int damageOnHit;
	public bool canDamage;
	public bool destroyOnCollision;
	public AudioClip sound;

	private bool isChecked;
	private bool readyToMove;
	private float distance;
	private GameObject playerObj;

	#endregion

	// Use this for initialization
	void Start () {
	
		canDamage = true;

		isChecked = false;

		readyToMove = false;

	}
	
	// Update is called once per frame
	void Update () {
	
		// Check if player is in the valid distance to move this prop
		playerObj = GameObject.Find("Player");

		distance = Vector3.Distance (gameObject.transform.position, playerObj.gameObject.transform.position);

		if (distance <= 2) {
			readyToMove = true;
		} else {
			readyToMove = false;
		}

		if (Input.GetKey (KeyCode.E) && readyToMove) {
			gameObject.transform.position += playerObj.gameObject.transform.forward * Time.deltaTime * moveSpeed;
		}
	}

	void getHit(int Damage){
		AudioSource.PlayClipAtPoint(sound,transform.position);
	}
		
	void OnCollisionEnter(Collision theObject){

		// When get hit by the blast, it will be pushed up to the air and at that time it has ability to damage zombie.
		if (theObject.gameObject.tag == "Blast") {
			canDamage = true;
		}

		// Prop only has damage while falling from a high position or flying, for any other case, it just works as a obstacle.
		if (theObject.gameObject.tag == "Floor") {
			canDamage = false;
		}

		// If collide with a zombie and it can apply damage
		if (theObject.gameObject.tag == "Zombie" && canDamage == true) {
			theObject.gameObject.GetComponent<Script_Zombie>().getPropHit(damageOnHit);
		}

		if (theObject.gameObject.tag == "Player") {   //&& Input.GetKey(KeyCode.E)
			gameObject.transform.forward = theObject.gameObject.transform.forward;
			gameObject.transform.position += gameObject.transform.forward * 0.5f;
		}

		if (theObject != null) AudioSource.PlayClipAtPoint(sound,transform.position);
		if (destroyOnCollision) Destroy (gameObject, 3);
	}
	

}

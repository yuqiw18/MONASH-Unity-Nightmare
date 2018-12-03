using UnityEngine;
using System.Collections;

public class Script_MainMenuEffects : MonoBehaviour {

	private bool moveForward;
	private bool rotateBack; 

	// Use this for initialization
	void Start () {
	
		moveForward = false;
		rotateBack = false;

	}
	
	// Update is called once per frame
	void Update () {
	
		if (moveForward) {
			if (gameObject.transform.position.z > -5) {
				gameObject.transform.position -= new Vector3 (0,0,Time.deltaTime * 50.0f);
			}
		} else {
			if (gameObject.transform.position.z < 12) {
				gameObject.transform.position += new Vector3 (0,0,Time.deltaTime * 50.0f);

			}
		}
	}


	public void FowardBackward(){
		moveForward = !moveForward;
	}


	public void RotateBack(){
		rotateBack = !rotateBack;
	}


}

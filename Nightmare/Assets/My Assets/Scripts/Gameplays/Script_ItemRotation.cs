using UnityEngine;
using System.Collections;

public class Script_ItemRotation: MonoBehaviour {

	public float xRot;
	public float yRot;
	public float zRot;
	public bool rotOnX;
	public bool rotOnY;
	public bool rotOnZ;


	// Use this for initialization
	void Start () {
	
		gameObject.transform.Rotate (new Vector3 (xRot, yRot, zRot));

	}

	void Update() {

		if (rotOnX) {
			gameObject.transform.Rotate(new Vector3(1,0,0),90 * Time.deltaTime);
		}

		if (rotOnY) {
			gameObject.transform.Rotate(new Vector3(0,1,0),90 * Time.deltaTime);
		}

		if (rotOnZ) {
			gameObject.transform.Rotate(new Vector3(0,0,1),90 * Time.deltaTime);
		}

	}
}

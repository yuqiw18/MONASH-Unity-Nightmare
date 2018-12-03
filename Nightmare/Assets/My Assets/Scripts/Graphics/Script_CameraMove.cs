using UnityEngine;
using System.Collections;

public class Script_CameraMove : MonoBehaviour {

	public float y;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (gameObject.transform.position.y < y) {
		
			gameObject.transform.position += new Vector3(0, Time.deltaTime * 0.25f, 0);
		
		}


	}
}

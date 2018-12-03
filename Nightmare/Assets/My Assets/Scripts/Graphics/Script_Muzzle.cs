using UnityEngine;
using System.Collections;

public class Script_Muzzle : MonoBehaviour {

	public float frequency;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		gameObject.transform.Rotate(new Vector3(1,0,0),frequency * Time.deltaTime);
	}
}

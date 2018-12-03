using UnityEngine;
using System.Collections;

public class Script_HauntedPainting : MonoBehaviour {

	public Material[] paintings;

	public float distance;

	private GameObject PlayerObj;

	// Use this for initialization
	void Start () {
	
		PlayerObj = GameObject.Find("Player");

	}
	
	// Update is called once per frame
	void Update () {
	
		if (Vector3.Distance (PlayerObj.gameObject.transform.position, gameObject.transform.position) >= distance) {
		
			gameObject.GetComponent<Renderer>().material = paintings [0];

		} else {
		
			gameObject.GetComponent<Renderer>().material = paintings [1];

		}

	}
}

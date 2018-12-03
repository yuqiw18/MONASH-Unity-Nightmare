using UnityEngine;
using System.Collections;

public class Script_Blast : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3 (0.1f , 0.1f , 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale += new Vector3 (1, 1, 1) * Time.deltaTime * 10; //Make it bigger
		if (transform.localScale.x > 10) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter(Collision theObject){ // Only physical props will get affected by the blast
		if (theObject.gameObject.tag != "Prop") {
			Physics.IgnoreCollision(theObject.collider, GetComponent<Collider>());
		}

	}
}

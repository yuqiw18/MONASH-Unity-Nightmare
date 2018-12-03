using UnityEngine;
using System.Collections;

public class Script_HitParticle : MonoBehaviour {

	private float wait = 0.4f;

	// Update is called once per frame
	void Update () {
		StartCoroutine (WaitAndDestroy());
	}
	
	IEnumerator WaitAndDestroy(){
		yield return new WaitForSeconds(wait);
		Destroy (gameObject);
	}
}

using UnityEngine;
using System.Collections;

public class Script_Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void ChangeMenu(){

		StartCoroutine (WaitAndShow ());

	}



	IEnumerator WaitAndShow(){
		yield return new WaitForSeconds (1.0f);
		gameObject.SetActive (true);

	}
}

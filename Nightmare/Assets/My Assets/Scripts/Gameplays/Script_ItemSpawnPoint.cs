using UnityEngine;
using System.Collections;

public class Script_ItemSpawnPoint : MonoBehaviour {

	#region Declare Variable

	public GameObject[] items;
	public bool pointActivated;
	private bool isEmpty;

	#endregion


	// Use this for initialization
	void Start () {
	
		pointActivated = true;
		isEmpty = true;

	}
	
	// Update is called once per frame
	void Update () {

		if (pointActivated) {
		
			// If there is no item on the spawn point (already picked by player), generate a new one. Otherwise, keep the old one.
			if (isEmpty){

				int i = Random.Range(0, items.Length);
				
				Instantiate(items[i],transform.position,Quaternion.identity);

				isEmpty = false;

			}
				pointActivated = false;

		}
	
	}



	void OnTriggerEnter(Collider theObject){

		// Item is picked by player
		if (theObject.gameObject.tag == "Player") {

			isEmpty = true;
		
		}

	}

}

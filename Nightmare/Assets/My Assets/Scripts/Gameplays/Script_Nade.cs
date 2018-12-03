using UnityEngine;
using System.Collections;

public class Script_Nade : MonoBehaviour {

	public GameObject blast;
	public GameObject explosion;
	public AudioClip noise;

	private int damage;

	void OnCollisionEnter(Collision theObject){

		// Explode when hitting the zombie or dropping down to the ground
		if (theObject.gameObject.tag == "Zombie" || theObject.gameObject.tag == "Floor" || theObject.gameObject.tag == "Structure") {
			AudioSource.PlayClipAtPoint(noise,transform.position,3f);
			GameObject boom = Instantiate(explosion) as GameObject;
			boom.transform.position = transform.position;
			theObject.gameObject.transform.SendMessage ("getHit", damage, SendMessageOptions.DontRequireReceiver); //Call the method Apply Damage in the gameobject that is hit
			Instantiate(blast, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
		
	}

	public void setDamage(int value){
		damage = value;
	}

}

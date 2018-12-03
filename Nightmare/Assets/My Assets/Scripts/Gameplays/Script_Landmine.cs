using UnityEngine;
using System.Collections;

public class Script_Landmine : MonoBehaviour {

	public GameObject blast;
	public GameObject explosion;
	public AudioClip noise;
	private int damage;
	private int durability;

	// Use this for initialization
	void Start () {
		durability = 100;
	}
	
	// Update is called once per frame
	void Update () {

		// Detonate the landmine if player shoot too much at it
		if (durability <= 0) {
			AudioSource.PlayClipAtPoint(noise,transform.position,3f);
			GameObject boom = Instantiate(explosion) as GameObject;
			boom.transform.position = transform.position;
			Instantiate(blast, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
	
	void OnCollisionEnter(Collision theObject){

		// Detonate the landmin when getting affected by the blast from other explosions
		if (theObject.gameObject.tag == "Blast" && theObject != null) {
			getHit (100);
		}

		// Detonate the landmine when colliding with zombie
		if (theObject.gameObject.tag == "Zombie") {
			AudioSource.PlayClipAtPoint(noise,transform.position,3f);
			GameObject boom = Instantiate(explosion) as GameObject;
			boom.transform.position = transform.position;
			theObject.gameObject.transform.SendMessage ("getHit", damage, SendMessageOptions.DontRequireReceiver); //Call the method Apply Damage in the gameobject that is hit
			Instantiate(blast, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}

	void getHit(int Damage){
		durability -= Damage;
	}

	public void setDamage(int value){
		damage = value;
	}

}

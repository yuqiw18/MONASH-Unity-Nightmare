using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Script_Structure : MonoBehaviour {

	// This script is now applied to the object that player needs to protect

	#region Declare variable

	public float durability;
	public GameObject[] damagedStructure;
	public AudioClip hitSound;
	private float maxDurability;
	private float damageProgress;
	private int damageIndex;
	private bool gettingDamage;
	private float gettingDamageTimer;
	private GameObject PlayerObj;
	private GameObject GUIObj;

	#endregion 


	// Use this for initialization
	void Start () {

		maxDurability = durability;
		damageProgress = maxDurability / (damagedStructure.Length + 1);
		gettingDamage = false;
	
	}
	
	// Update is called once per frame
	void Update () {

		// Change the lookings based on how much damage it takes

		damageIndex = (int)((maxDurability - durability)/damageProgress);

		if (damageIndex >= 1 && damageIndex <= damagedStructure.Length) {

			damagedStructure [damageIndex-1].gameObject.SetActive(true);

		}

		if (durability <= 0) { // If it is destroyed

			durability = 0;
			PlayerObj = GameObject.Find("Player");
			PlayerObj.gameObject.GetComponent<Script_PlayerStatus>().ChangeLevel("End"); // Game Over

		}

		if (gettingDamage) {
		
			if (Time.time > gettingDamageTimer){

				gettingDamageTimer = Time.time + 3.0f;
				
				gettingDamage = false;
			}else{

				durability -= Time.deltaTime * 2.0f;

			}
		}
	}

	// Damages from the bullet and melee attack
	void getHit(int Damage){
		durability -= Damage/25;
	}

	void OnCollisionEnter(Collision theObject){

		// Damages from the blast
		if (theObject.gameObject.tag == "Blast" && theObject != null) {

			durability -= (15 + Random.Range (-5, 6));
		}

		// Damages from the zombie
		if (theObject.gameObject.tag == "Zombie") {

			durability -= 3;

			gettingDamage = true;
		
		}
	}

	void OnGUI(){

		GUIObj = GameObject.Find ("GUI_StructureHP");
		GUIObj.gameObject.GetComponent<Text>().text = ((int)durability).ToString ();
	
	}
	
}

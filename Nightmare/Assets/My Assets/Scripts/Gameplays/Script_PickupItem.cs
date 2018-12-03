using UnityEngine;
using System.Collections;

public class Script_PickupItem : MonoBehaviour {

	#region Declare Variables

	public enum pickupTypes {SMG,Rifle,Ammo,Food,Drinks}
	public pickupTypes pickupType;
	public AudioClip pickupSound;

	private GameObject Obj;//Used to locate the object we want
	
	#endregion
	
	void OnTriggerEnter(Collider theObject){
		if (theObject.gameObject.tag == "Player") {

			if (pickupType == pickupTypes.SMG) {
				Obj = GameObject.Find ("SMG");//Find the weapon
				if (Obj.gameObject.GetComponent<Script_Weapon> ().weaponAcquired == true) {//If we already have the weapon
					Obj.gameObject.GetComponent<Script_Weapon> ().GetAmmo (2);//Get 2 mags of ammo
				} else {
					Obj.gameObject.GetComponent<Script_Weapon> ().weaponAcquired = true;//We pick up a new gun
				}

			}else if (pickupType == pickupTypes.Rifle) {
				Obj = GameObject.Find ("Rifle");//Find the weapon
				if (Obj.gameObject.GetComponent<Script_Weapon> ().weaponAcquired == true) {//If we already have the weapon
					Obj.gameObject.GetComponent<Script_Weapon> ().GetAmmo (2);//Get 2 mags of ammo
					} else {
					Obj.gameObject.GetComponent<Script_Weapon> ().weaponAcquired = true;//We pick up a new gun
					}

			} else if (pickupType == pickupTypes.Ammo) {

				// Get each types of ammo for currently acquired weapons
				Obj = GameObject.Find ("Pistol");
				Obj.gameObject.GetComponent<Script_Weapon> ().GetAmmo (2);
	
				Obj = GameObject.Find ("SMG");
				if (Obj.gameObject.GetComponent<Script_Weapon> ().weaponAcquired == true) {
					Obj.gameObject.GetComponent<Script_Weapon> ().GetAmmo (1);
				}
				Obj = GameObject.Find ("Rifle");
				if (Obj.gameObject.GetComponent<Script_Weapon> ().weaponAcquired == true) {
					Obj.gameObject.GetComponent<Script_Weapon> ().GetAmmo (1);
				}
			}else if (pickupType == pickupTypes.Food){
				Obj = GameObject.Find ("Player");
				Obj.gameObject.GetComponent<Script_Inventory>().food += 1;
			}else if (pickupType == pickupTypes.Drinks){
				Obj = GameObject.Find ("Player");
				Obj.gameObject.GetComponent<Script_Inventory>().drinks += 1;
			}
			AudioSource.PlayClipAtPoint(pickupSound,transform.position);
			Destroy (gameObject);
		}
	}
}

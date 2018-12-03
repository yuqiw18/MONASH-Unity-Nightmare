using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Script_Inventory : MonoBehaviour {

	#region Declare variables

	//private GameObject currentWeapon;
	public GameObject[] weaponsInventory;
	public int food;
	public int drinks;

	private GameObject Obj;

	#endregion

	// Use this for initialization
	void Start () {
		SwitchWeapon (1); //Switch to default pistol
	}

	// Update is called once per frame
	void Update () {

		#region Switch weapon
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			SwitchWeapon(0);
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			SwitchWeapon (1);
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			SwitchWeapon (2);
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			SwitchWeapon(3);
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			SwitchWeapon (4);
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			SwitchWeapon (5);
		}
		#endregion

		#region Use item

		if (Input.GetKeyDown (KeyCode.Q)) {
			UseItem(0);
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			UseItem(1);
		}

		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			UseItem(2);
		}

		#endregion

	}

	public void SwitchWeapon(int i){
		if (weaponsInventory [i].gameObject.GetComponent<Script_Weapon> ().weaponAcquired == true) { //If the weapon is acquired
			for (int j =0; j< weaponsInventory.Length; j++) {
				weaponsInventory[j].GetComponent<Renderer>().enabled = false;
				weaponsInventory[j].gameObject.GetComponent<Script_Weapon>().weaponActivated = false; //Disable any other weapon
			}
		weaponsInventory[i].gameObject.GetComponent<Script_Weapon>().weaponActivated = true; //Activate this weapon
		//currentWeapon = weaponsInventory[i];
		weaponsInventory[i].GetComponent<Renderer>().enabled = true;
		}
	}

	void UseItem(int i){
		switch (i) {
		case 0:
			if (food >= 1){
				Obj = GameObject.Find("Player");
				Obj.gameObject.GetComponent<Script_PlayerStatus>().health += 30;
				food -= 1;
			}
			break;
		case 1:
			if (drinks >= 1){
				Obj = GameObject.Find("Player");
				Obj.gameObject.GetComponent<Script_PlayerStatus>().health += 15;
				Obj = GameObject.Find("First Person Controller");
				Obj.gameObject.GetComponent<Script_PlayerMovement>().stamina += 15;
				drinks -= 1;
			}
			break;
		default:
			break;
		}
	}


	void OnGUI(){
		Obj = GameObject.Find("GUI_FoodNum");
		Obj.gameObject.GetComponent<Text> ().text = "Food: " + food.ToString();

		Obj = GameObject.Find("GUI_DrinksNum");
		Obj.gameObject.GetComponent<Text> ().text = "Drinks: " + drinks.ToString();
	}

}

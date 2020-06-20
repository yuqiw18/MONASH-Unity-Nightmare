using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Script_Weapon : MonoBehaviour {


	#region Declare Variables

	public enum weaponTypes{Melee, Pistol, SMG, Rifle, Nade, Landmine}

	public weaponTypes weaponType;

	public bool weaponAcquired = false;
	public bool weaponActivated = false;

	private int currentAmmo;
	private int allAmmo;
	public int magAmmo;
	public int maxAmmo;

	public Rigidbody ammoType; // Leave empty for any type of gun

	public float fireRate;
	private float wait = 0;
	public float recoilValue;
	public int Damage;
	public int damageModifier;  
	public float reloadingTime;

	private float recoilEffect;
	private bool changingMag;
	private bool canShoot;

	public GameObject hitEffect;
	public AudioClip gunFire; // Gunfire sounds
	public AudioClip reloading; // Reloading sounds

	public Texture2D crossHair;
	Rect position; // Used to display the crosshair

	public Light muzzleLight;
	public GameObject muzzleFlash;
	
	private GameObject hitObject; // Used to check the object that gets hit
	private GameObject hitParticle;  // Used to instantiate hit particle effect
	private GameObject GUIObj; // Used to find the GUI elements
	private string weaponName;

	#endregion
	
	// Use this for initialization
	void Start () {

		canShoot = true;
		changingMag = false;
		currentAmmo = magAmmo;
		allAmmo = maxAmmo;
		recoilEffect = 0;

	}
	
	// Update is called once per frame
	void Update () {

		// Always show the crosshair at the center of screen
		position = new Rect((Screen.width - crossHair.width) / 2, (Screen.height - crossHair.height) /2, crossHair.width, crossHair.height);

		Melee ();
		Recoil ();
		Shoot ();
		ThrowNade ();
		DeployLandmine();
		Reload ();
		Check ();

	}


	#region GUI Elements

	void OnGUI(){
		if (weaponActivated == true){
			if (weaponType != weaponTypes.Melee) {
				GUIObj = GameObject.Find ("GUI_AmmoNum");
				GUIObj.gameObject.GetComponent<Text>().text = currentAmmo.ToString () + " / " + allAmmo.ToString();

			}else{
				GUIObj = GameObject.Find ("GUI_AmmoNum");
				GUIObj.gameObject.GetComponent<Text>().text = "Ø / Ø";
			}

			ShowWeaponIcon();

			GUIObj = GameObject.Find ("GUI_WeaponName");
			weaponName = weaponType.ToString();
			if (weaponType == weaponTypes.Melee){
				weaponName = "Bat";
			}
			if (weaponType == weaponTypes.Nade){
				weaponName = "Grenade";
			}
			GUIObj.gameObject.GetComponent<Text>().text = weaponName;
			GUI.DrawTexture(position, crossHair);
		}
	}

	void ShowWeaponIcon(){
		if (weaponType == weaponTypes.Melee){
			GUIObj = GameObject.Find("GUI_Icon_Bat");
			GUIObj.gameObject.GetComponent<RawImage>().enabled = true;
		}else
		{
			GUIObj = GameObject.Find("GUI_Icon_Bat");
			GUIObj.gameObject.GetComponent<RawImage>().enabled = false;
		}
		
		if (weaponType == weaponTypes.Pistol){
			GUIObj = GameObject.Find("GUI_Icon_Pistol");
			GUIObj.gameObject.GetComponent<RawImage>().enabled = true;
		}else
		{
			GUIObj = GameObject.Find("GUI_Icon_Pistol");
			GUIObj.gameObject.GetComponent<RawImage>().enabled = false;
		}
		
		if (weaponType == weaponTypes.SMG){
			GUIObj = GameObject.Find("GUI_Icon_SMG");
			GUIObj.gameObject.GetComponent<RawImage>().enabled = true;
		}else
		{
			GUIObj = GameObject.Find("GUI_Icon_SMG");
			GUIObj.gameObject.GetComponent<RawImage>().enabled = false;
		}
		
		if (weaponType == weaponTypes.Rifle){
			GUIObj = GameObject.Find("GUI_Icon_Rifle");
			GUIObj.gameObject.GetComponent<RawImage>().enabled = true;
		}else
		{
			GUIObj = GameObject.Find("GUI_Icon_Rifle");
			GUIObj.gameObject.GetComponent<RawImage>().enabled = false;
		}
		
		if (weaponType == weaponTypes.Nade){
			GUIObj = GameObject.Find("GUI_Icon_Nade");
			GUIObj.gameObject.GetComponent<RawImage>().enabled = true;
		}else
		{
			GUIObj = GameObject.Find("GUI_Icon_Nade");
			GUIObj.gameObject.GetComponent<RawImage>().enabled = false;
		}
		
		if (weaponType == weaponTypes.Landmine){
			GUIObj = GameObject.Find("GUI_Icon_Landmine");
			GUIObj.gameObject.GetComponent<RawImage>().enabled = true;
		}else
		{
			GUIObj = GameObject.Find("GUI_Icon_Landmine");
			GUIObj.gameObject.GetComponent<RawImage>().enabled = false;
		}
	}


	#endregion

	#region Melee
	void Melee(){
		if (weaponType == weaponTypes.Melee) {
			if (Input.GetKeyUp (KeyCode.Mouse0) && weaponActivated == true && canShoot){
				Ray ray = new Ray (Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward);
				RaycastHit hit;
				Debug.DrawRay (ray.origin, ray.direction , Color.red, 3f);
				GetComponent<Animation>().Play("Melee");
				if (Physics.Raycast (ray, out hit, 4)) {
					hitObject = hit.collider.gameObject;
					AudioSource.PlayClipAtPoint (gunFire, transform.position);
					hit.transform.SendMessage ("getHit", Damage + Random.Range(damageModifier*-1 , damageModifier+1), SendMessageOptions.DontRequireReceiver);
				}else{
					AudioSource.PlayClipAtPoint (reloading, transform.position);
				}
				StartCoroutine(Wait(fireRate));
			}
		}
	}
	#endregion
	
	#region Shoot
	void Shoot(){
		//Semi-Auto weapon
		if (weaponType == weaponTypes.Rifle || weaponType == weaponTypes.Pistol) {
			if (Input.GetKeyUp (KeyCode.Mouse0) && weaponActivated == true && currentAmmo !=0 && Time.time > wait) {//&& changingMag != true
				//Play the sound
				AudioSource.PlayClipAtPoint (gunFire, transform.position);
				// Delay the shot
				wait = Time.time + fireRate;
				//Create a bullet and shoot it off from the center of the screen with a direction based on the recoil
				//Ray ray = new Ray (Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward + new Vector3(Random.Range(0,recoilEffect/2),recoilEffect,Random.Range(0,recoilEffect/2)));
				Ray ray = new Ray (Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward);
				RaycastHit hit;
				Debug.DrawRay (ray.origin, ray.direction * 25f, Color.red, 3f);

				if (weaponType == weaponTypes.Pistol){
					GetComponent<Animation>().Play("Pistol");
				}

				if (weaponType == weaponTypes.Rifle){
					GetComponent<Animation>().Play("Rifle");
				}

				currentAmmo -= 1;
				recoilEffect += Random.Range(recoilValue/2,recoilValue*1.5f);

				if (Physics.Raycast (ray, out hit, 500)) {
					
					hitObject = hit.collider.gameObject;
					hitParticle = Instantiate (hitEffect, hit.point, Quaternion.LookRotation (hit.normal));
					//ParticleSystem particle = Instantiate (hitEffect, hit.point, Quaternion.LookRotation (hit.normal)) as ParticleSystem;
					hit.transform.SendMessage ("getHit", Damage + Random.Range(damageModifier*-1 , damageModifier+1), SendMessageOptions.DontRequireReceiver);
					hit.transform.SendMessage ("getCriticalHit", Damage + Random.Range(damageModifier*-1 , damageModifier+1), SendMessageOptions.DontRequireReceiver);
				}

				// Show the muzzle flash and muzzle light
				StartCoroutine(Muzzle());

				// Delay the next shot
				//StartCoroutine(Wait(fireRate));

			}
			
		}
		
		//Auto weapon
		if (weaponType == weaponTypes.SMG) {
			if (Input.GetButton ("Fire1") && weaponActivated == true && currentAmmo !=0 && Time.time > wait) {//&& changingMag != true
				//Play the sound
				AudioSource.PlayClipAtPoint (gunFire, transform.position);
				//Delay the shot
				wait = Time.time + fireRate;
				//Create the ray
				Ray ray = new Ray (Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward  + new Vector3(Random.Range(0,recoilEffect/2),recoilEffect,Random.Range(0,recoilEffect/2)));
				RaycastHit hit;
				
				Debug.DrawRay (ray.origin, ray.direction * 25f, Color.red, 3f);
				GetComponent<Animation>().Play("SMG");
				//Decrease the ammo by 1
				currentAmmo -= 1;
				recoilEffect += Random.Range(recoilValue/2,recoilValue*1.5f);
				//Check collision, range 1000 units
				if (Physics.Raycast (ray, out hit, 500)) { 

					hitObject = hit.collider.gameObject;
					hitParticle = Instantiate (hitEffect, hit.point, Quaternion.LookRotation (hit.normal));
					hit.transform.SendMessage ("getHit", Damage + Random.Range(damageModifier*-1 , damageModifier+1), SendMessageOptions.DontRequireReceiver); //Call the method Apply Damage in the gameobject that is hit
					hit.transform.SendMessage ("getCriticalHit", Damage + Random.Range(damageModifier*-1 , damageModifier+1), SendMessageOptions.DontRequireReceiver);
				}
				StartCoroutine(Muzzle());
				//StartCoroutine(Wait(fireRate));
			}	
		}
	}
	#endregion

	#region Reload and Get Ammo
	void Reload(){
		if (weaponType != weaponTypes.Melee && weaponActivated != false && Time.time > wait) {
			if (currentAmmo == 0 && allAmmo > 0) {
				if (allAmmo >= magAmmo) {
					wait = Time.time + reloadingTime;
					//StartCoroutine(Wait(reloadingTime));
					changingMag = true;
					currentAmmo = magAmmo;
					allAmmo -= magAmmo;
					changingMag = false;
				}else {
					changingMag = true;
					wait = Time.time + reloadingTime;
					//StartCoroutine(Wait(reloadingTime));
					currentAmmo = allAmmo;
					allAmmo = 0;
					changingMag = false;
				}
				if (weaponType != weaponTypes.Landmine && weaponType != weaponTypes.Nade) {
					AudioSource.PlayClipAtPoint (reloading, transform.position);
				}
				recoilEffect = 0;
				ReloadAnimation();
			}
		}
		if (weaponType != weaponTypes.Melee && weaponActivated != false && Input.GetKeyDown (KeyCode.R) && currentAmmo != 0 && currentAmmo < magAmmo && allAmmo > 0 && !changingMag) {
			if (weaponType != weaponTypes.Landmine && weaponType != weaponTypes.Nade) {
				AudioSource.PlayClipAtPoint (reloading, transform.position);
			}
			if (currentAmmo + allAmmo >= magAmmo) {
				wait = Time.time + reloadingTime;
				//StartCoroutine(Wait(reloadingTime));
				changingMag = true;
				int ammoRemaining = magAmmo - currentAmmo;
				currentAmmo = magAmmo;
				allAmmo -= ammoRemaining;
				changingMag = false;
				
			} else {
				changingMag = true;
				wait = Time.time + reloadingTime;
				//StartCoroutine(Wait(reloadingTime));
				currentAmmo += allAmmo;
				allAmmo = 0;
				changingMag = false;
			}
			recoilEffect = 0;
			ReloadAnimation();
		}
	}
	
	public void GetAmmo (int mag){
		if (weaponType != weaponTypes.Melee) {
			allAmmo += mag * magAmmo;
			if ((currentAmmo + allAmmo) > (maxAmmo + magAmmo)) {
				allAmmo -= ((currentAmmo + allAmmo) - (maxAmmo + magAmmo));//The total number of ammo should always be the magzine + maximum in inventory
			}
		}
	}
	#endregion

	#region Landmin & Grenade
	void DeployLandmine(){
		if (weaponType == weaponTypes.Landmine) {
			if (Input.GetKeyUp(KeyCode.Mouse0) && weaponActivated == true && currentAmmo !=0 && canShoot){//&& changingMag != true
				//Play the sound
				AudioSource.PlayClipAtPoint(gunFire,transform.position);
				//Deploy the landmine
				Rigidbody g = Instantiate (ammoType,transform.position, Quaternion.identity) as Rigidbody;
				g.gameObject.GetComponent<Script_Landmine>().setDamage(Damage + Random.Range (damageModifier*-1,damageModifier+1));
				Physics.IgnoreCollision (g.GetComponent<Collider>(),transform.root.GetComponent<Collider>());
				currentAmmo -= 1;
				StartCoroutine(Wait(fireRate));
			}
		
		}
			
	}
	
	void ThrowNade(){
		if (weaponType == weaponTypes.Nade) {
			if (Input.GetKeyUp(KeyCode.Mouse0) && weaponActivated == true && currentAmmo !=0 && canShoot){//&& changingMag != true
				//Play the sound
				AudioSource.PlayClipAtPoint(gunFire,transform.position);
				//Deploy the landmine
				Rigidbody g = Instantiate (ammoType,transform.position,transform.rotation) as Rigidbody;//
				g.gameObject.GetComponent<Script_Nade>().setDamage(Damage + Random.Range (damageModifier*-1,damageModifier+1));
				g.velocity = transform.TransformDirection(new Vector3 (0,0,15));
				Physics.IgnoreCollision (g.GetComponent<Collider>(),transform.root.GetComponent<Collider>());
				currentAmmo -= 1;
				StartCoroutine(Wait(fireRate));
			}
			
		}

	}
	
	void Check(){ // Check if player runs out of grenades or landmines. Unlike guns or melee weapon, they can't be kept.
		if (weaponActivated == true && currentAmmo == 0 && allAmmo == 0 && (weaponType == weaponTypes.Nade || weaponType == weaponTypes.Landmine)) {//When runing out of it
			weaponAcquired = false;
			GameObject f = GameObject.Find("Player");
			f.gameObject.GetComponent<Script_Inventory>().SwitchWeapon(0);//Switch to melee weapon
		}
	}
	#endregion
	
	#region Weapon Animations
	void ReloadAnimation(){
		if (weaponType == weaponTypes.Pistol){
			GetComponent<Animation>().Play("Pistol_R");
		}

		if (weaponType == weaponTypes.SMG){
			GetComponent<Animation>().Play("SMG_R");
		}

		if (weaponType == weaponTypes.Rifle){
			GetComponent<Animation>().Play("Rifle_R");
		}
	}
	#endregion

	#region Recoil

	void Recoil(){
	
		// Recoil effect cant smaller than zero
		if (recoilEffect < 0) {
			recoilEffect = 0;
		}

		// Recoil effect decreases while stop shooting
		if (!Input.GetKey(KeyCode.Mouse0) &&recoilEffect > 0) {
			recoilEffect -= Time.deltaTime * recoilValue * 20;
		}
	
	}

	#endregion
	
	IEnumerator Muzzle(){
		muzzleLight.gameObject.GetComponent<Light>().intensity = Random.Range (2, 5);
		muzzleLight.gameObject.GetComponent<Light>().enabled = true;
		muzzleFlash.gameObject.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(0.05f);
		muzzleFlash.gameObject.GetComponent<Renderer>().enabled = false;
		muzzleLight.gameObject.GetComponent<Light>().enabled = false;
	}
	
	IEnumerator Wait(float time){
		canShoot = false;
		yield return new WaitForSeconds(time);
		canShoot = true;
	}

}

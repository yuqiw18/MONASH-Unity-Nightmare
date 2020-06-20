using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Script_MapConfig : MonoBehaviour {

	#region Declare Variables

	public enum gameModes{Survival,Defense}
	public gameModes gameMode;

	// Survival Mode Variables
	public int waveNum;
	private int waveCount;
	public float gapTime;
	private bool awarded;
	private float nextWave;
	private GameObject GUIObj;
	private int zombieKilledThisWave;
	private int totalZombieKilled;
	private float gapWait;
	private GameObject reseter;

	// Defense Mode Variables


	private float countDown;
	private float totalTime;
	private int min;
	private int sec;
	private string sec2;
	private GameObject Obj;

	public GameObject[] zombieSpawnPoints;
	public GameObject[] itemSpawnPoints;
	private bool zombieSpawnlocked;
	private bool itemSpawnLocked;

	private bool gameOver;

	#endregion
	// Use this for initialization
	void Start () {

		Cursor.visible = false;
		gameOver = false;

		// Survival
		waveCount = 1;
		nextWave = 0;
		zombieSpawnlocked = true;
		itemSpawnLocked = false;
		awarded = false;

		// Defense
		totalTime = 300; // In seconds

	}
	
	// Update is called once per frame
	void Update () {

		
		if (gameMode == gameModes.Survival) {
			
			// When not reach the max number of waves
			
			if (waveCount < waveNum) {
				
				// If killed all the zombies in this wave
				
				if (zombieKilledThisWave == 10 * waveCount && zombieSpawnlocked) {


					if (zombieKilledThisWave%20 == 0 && !awarded){

						Obj = GameObject.Find ("Nade");//Find the weapon
						if (Obj.gameObject.GetComponent<Script_Weapon> ().weaponAcquired == true) {//If we already have the weapon
							Obj.gameObject.GetComponent<Script_Weapon> ().GetAmmo (2);//Get 2 mags of ammo
						} else {
							Obj.gameObject.GetComponent<Script_Weapon> ().weaponAcquired = true;//We get new grenade
							Obj.gameObject.GetComponent<Script_Weapon> ().GetAmmo (5);
						}
						
						
						Obj = GameObject.Find ("Landmine");//Find the weapon
						if (Obj.gameObject.GetComponent<Script_Weapon> ().weaponAcquired == true) {//If we already have the weapon
							Obj.gameObject.GetComponent<Script_Weapon> ().GetAmmo (2);//Get 2 mags of ammo
						} else {
							Obj.gameObject.GetComponent<Script_Weapon> ().weaponAcquired = true;//We get new landmine
							Obj.gameObject.GetComponent<Script_Weapon> ().GetAmmo (5);
						}

						awarded = true;
					}

					//

					zombieSpawnlocked = false;
					
					// Wait for gap time
					gapWait = Time.time + gapTime;
					
					// Regenerate item for item spawn points
					if (itemSpawnLocked == false){
						regenerateItem();
					}
					
					// Then lock it
					itemSpawnLocked = true;
					
				}
				
				// If gap time countdown is finished
				
				if (Time.time >= gapWait && !zombieSpawnlocked) {
					
					// Respawn zombies
					
					respawnZombie();
					
					waveCount += 1;
					
					zombieSpawnlocked = true;
					
					itemSpawnLocked = false;

					awarded = false;
				}
				
			} else {
				
				if (waveNum == waveNum && zombieKilledThisWave == 10 * waveCount && zombieSpawnlocked){

					Obj = GameObject.Find("Player");
					Obj.gameObject.GetComponent<Script_PlayerStatus>().ChangeLevel("End_2"); // Win!

				}
					
			}
		}
		
		
		if (gameMode == gameModes.Defense) {
			
			// Calculate the time and display it in hh:mm format
			
			countDown = totalTime - Time.time;
			min = (int)(countDown/60);
			sec = (int)(countDown%60);
			
			if ((int)(sec/10) == 0){
				sec2 = "0" + sec.ToString();
			}else{
				sec2 = sec.ToString();
			}
			
			// Modify the zombie properties based on time elapse
			
			modifyZombie();
			
			// Close garage door in last 20 secs
			if (min == 0 && sec == 40){
				
				
				Obj = GameObject.Find("WarehouseDoor");
				Obj.gameObject.GetComponent<Script_GarageDoor>().on = true;
				
			}

			if (min ==0 && sec == 0){

				Obj = GameObject.Find("Player");
				Obj.gameObject.GetComponent<Script_PlayerStatus>().ChangeLevel("End_2"); // Win!

			}
			
		}

	}
	
	void OnGUI(){

		if (gameMode == gameModes.Survival) {

			GUIObj = GameObject.Find ("GUI_WaveNum");
			GUIObj.gameObject.GetComponent<Text> ().text = "Wave: " + waveCount.ToString();
			
			GUIObj = GameObject.Find ("GUI_Score");
			GUIObj.gameObject.GetComponent<Text> ().text = zombieKilledThisWave.ToString();

			GUIObj = GameObject.Find ("GUI_StructureHP");
			GUIObj.gameObject.GetComponent<Text> ().text = "";
			
			if (gapWait - Time.time >= 0) {
				GUIObj = GameObject.Find ("GUI_CountDown");
				GUIObj.gameObject.GetComponent<Text> ().text =  "Next wave in: " + ((int)(gapWait - Time.time)).ToString ();
			} else {
				GUIObj = GameObject.Find ("GUI_CountDown");
				GUIObj.gameObject.GetComponent<Text> ().text = "";
			}
		}

		if (gameMode == gameModes.Defense) {
		
			GUIObj = GameObject.Find ("GUI_CountDown");
			GUIObj.gameObject.GetComponent<Text> ().text = "Time: " + min.ToString() + ":" + sec2;

			// Hide wave number because it is not used in defense mode
			GUIObj = GameObject.Find ("GUI_WaveNum");
			GUIObj.gameObject.GetComponent<Text> ().text = "";

			GUIObj = GameObject.Find ("GUI_Score");
			GUIObj.gameObject.GetComponent<Text> ().text = zombieKilledThisWave.ToString();
		}


	}


	#region Respawn methods for Survival Mode

	void respawnZombie(){
		for (int i = 0; i<zombieSpawnPoints.Length; i++) {
			zombieSpawnPoints [i].gameObject.GetComponent<Script_ZombieSpawnPoint> ().IncreaseWaveNum ();
			zombieSpawnPoints [i].gameObject.GetComponent<Script_ZombieSpawnPoint> ().ResetCount ();
		}
		
		totalZombieKilled += zombieKilledThisWave;
		
		resetZombieKilled ();
	}

	void regenerateItem(){
		for (int i = 0; i<itemSpawnPoints.Length; i++) {
			if (itemSpawnPoints [i].gameObject.GetComponent<Script_ItemSpawnPoint> ().pointActivated != true){
				itemSpawnPoints [i].gameObject.GetComponent<Script_ItemSpawnPoint> ().pointActivated = true;
			}

		}

	}
	
	public void zombieKilled(){

		zombieKilledThisWave += 1;
	}

	public void resetZombieKilled(){

		zombieKilledThisWave = 0;
	}

	#endregion


	#region Respawn methods for Defense Mode

	void modifyZombie(){

		for (int i = 0; i<zombieSpawnPoints.Length; i++) {
			zombieSpawnPoints [i].gameObject.GetComponent<Script_ZombieSpawnPoint> ().SetWaveNum(((int)(totalTime/60)-min)*2);
			zombieSpawnPoints [i].gameObject.GetComponent<Script_ZombieSpawnPoint> ().ResetCount ();
		}

	}
	#endregion


}

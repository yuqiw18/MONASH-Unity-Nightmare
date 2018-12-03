using UnityEngine;
using System.Collections;

public class Script_ZombieSpawnPoint : MonoBehaviour {

	#region declare variables

	public GameObject zombie;
	public int spawnActivatedAfterWaveNum;
	public float zombieStart;
	public float zombieSpawnRate;

	private int zombieNum;	//Number of zombies for this spawn point
	private int zombieCount;
	private GameObject enemyPlacement;
	private int currentWaveNum;

	#endregion
	
	// Use this for initialization
	void Start () {
		zombieNum = 10;
		zombieCount = 0;
		currentWaveNum = 1;
		//if (zombieCount <= zombieNum) {
		InvokeRepeating ("makeZombie", zombieStart, zombieSpawnRate);
		//}

	}
	
	void makeZombie () {

		if (zombieCount < zombieNum && currentWaveNum >= spawnActivatedAfterWaveNum) {
			enemyPlacement = Instantiate (zombie, transform.position, Quaternion.identity) as GameObject;
			zombieCount ++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		zombie.gameObject.GetComponent<Script_Zombie> ().waveNum = currentWaveNum;
	}

	#region Methods for Survival Mode
	public void IncreaseWaveNum(){

		currentWaveNum += 1;
	}

	public void ResetCount(){

		zombieCount = 0;
	}
	#endregion
	
	#region Methods for Defense Mode
	public void SetWaveNum(int value){

		currentWaveNum = value;

	}

	#endregion
	
}

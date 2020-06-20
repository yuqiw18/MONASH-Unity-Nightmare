using UnityEngine;
using System.Collections;

public class Script_Zombie : MonoBehaviour {

	#region Declare Variables
	private Transform target;
	public Transform[] allTargets; // A zombie may have multiple targets
	private UnityEngine.AI.NavMeshAgent agent;
	public int health;
	public int damage;
	public int waveNum ;
	public AudioClip zombieWalk;
	public AudioClip[] zombieDieSound;
	public AudioClip[] zombieSound;
	public GameObject healthBar;
	public Animation animationControl;

	public float moveSpeed = 4f;
	private bool zombieDied;
	private bool soundPlayed;
	private Vector3 spotLocation;
	private Quaternion lookHeading;
	private GameObject Obj;

	#endregion
	
	// Use this for initialization
	void Start () {

		target = allTargets [Random.Range (0, allTargets.Length)];
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		lookHeading = Quaternion.Euler (Vector3.zero);
		damage = Random.Range(5,16);
		health += 15 * (waveNum - 1);
		damage += 3 * (waveNum - 1);
		moveSpeed += 0.25f * (waveNum - 1);
		soundPlayed = false;
		zombieDied = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (health <= 0) {
			agent.updatePosition = false;
			if (!soundPlayed){
				int i = Random.Range (0, zombieDieSound.Length);
				AudioSource.PlayClipAtPoint (zombieDieSound [i], transform.position);
				soundPlayed = true;
			}

			if (!zombieDied){
				Obj = GameObject.Find ("Map Controller");
				Obj.gameObject.GetComponent<Script_MapConfig>().zombieKilled();
				zombieDied = true;
				animationControl.Play("back_fall", PlayMode.StopAll);
			}

			Destroy (gameObject, 1);
		
		} else {

			approachTarget ();

			if (!target.Equals("Player") && (transform.position - target.transform.position).magnitude < 4)
			{
				animationControl.Play("attack", PlayMode.StopAll);
			}

		}
	}
	
	void approachTarget(){
		agent.updatePosition = true;
		agent.destination = target.position;
		agent.speed = moveSpeed;
		//lookHeading = Quaternion.LookRotation (agent.velocity.normalized);
	}
	
	void getHit(int Damage){
		health -= Damage;
	}

	void getBlasted(int Damage){
		health -= Damage;
	}
	
	public void getPropHit(int Damage){
		health -= Damage;
	}

	#region Collision Detection

	void OnCollisionEnter(Collision theObject){
		if (theObject.gameObject.tag == "Blast" && theObject != null) {
			getBlasted (65 + Random.Range(-15, 16));//
		}

		if (theObject.gameObject.tag == "Zombie") {
			Physics.IgnoreCollision(theObject.collider, GetComponent<Collider>());
		}
	}
	
	void OnTriggerEnter(Collider theObject){
		if (theObject.gameObject.tag == "Player")
		{
			if (!zombieDied) 
			{
				theObject.gameObject.GetComponent<Script_PlayerStatus>().health -= (int)(damage / 4);
				theObject.gameObject.GetComponent<Script_PlayerStatus>().PlaySound();
				theObject.gameObject.GetComponent<Script_PlayerStatus>().losingHealth = true;
				animationControl.Play("attack", PlayMode.StopAll);
			}
		}
		else 
		{
			animationControl.Play("walk", PlayMode.StopAll);
		}
	}

	#endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;


public class Navi : TrueSyncBehaviour {
	const byte INPUT_DIRECTION = 0;
	const byte INPUT_BUSTER = 1;

	int requestDirection;
	float moveQueueWindow = 0.15f;
	FP moveCooldown = 0f;
	bool pendingMoveUp = false;
	bool pendingMoveDown = false;
	bool pendingMoveLeft = false;
	bool pendingMoveRight = false;

	int requestBuster = 0;
	float busterQueueWindow = 0.15f;
	FP busterCooldown = 0.25f;
	bool pendingBuster = false;



	public GameObject field;

	public int field_space_Local;
	public int field_space;	// location of navi on field
	public int next_space;	// locaiton navi will move to after move animation

	// buster info
	public float bust_charge = 0.0f;
	public float max_charge = 2.0f; // time in seconds for full charge
	bool charging = false;
	public GameObject charge_ring;
	public int bust_dmg = 1;
	public int charge1_dmg = 10;

	// HP
	public int HP = 100;
	public GameObject health_disp;

	public int playerNumber = 1;
	
	public int combo_color = 0;	// color of last chip used

	public GameObject shot_handler;
	public GameObject deck;

	Animator anim;

	void Awake(){
		anim = GetComponent<Animator> ();
		field = GameObject.Find ("Field");
		shot_handler = GameObject.Find("Shot Handler");
		if (localOwner.Id == owner.Id) { // If player owns this GO
			GameObject.Find ("Chip Bay").GetComponent<Chip_Hand> ().navi = this.gameObject;
			charge_ring = GameObject.Find("charge ring");

			GameObject.Find("Swiper").GetComponent<Swiper>().Navi = this;
			GameObject.Find("Buster Button").GetComponent<Buster>().Navi = this;
			GameObject.Find("Chip Bay").GetComponent<Chip_Hand>().navi = this.gameObject;
		}
	}
	// Use this for initialization
	public override void OnSyncedStart() {
		// Setting References		!! moved to Awake() !!
	//	if (localOwner.Id == owner.Id) { // If player owns this GO
	//		GameObject.Find ("Swiper").GetComponent<Swiper> ().Navi = this;
	//		GameObject.Find ("Buster Button").GetComponent<Buster> ().Navi = this;
	//		GameObject.Find ("Chip Bay").GetComponent<Chip_Hand> ().navi = this.gameObject;
	//	}
		// Setting the player's number for easy acess, and loading players into handlers
		if(owner.Id <= 1) {
			shot_handler.GetComponent<Shot_Handler>().playerA = this.transform.gameObject;
			health_disp = GameObject.Find("HealthA");
			playerNumber = 1;
		}
		if(owner.Id == 2) {
			shot_handler.GetComponent<Shot_Handler>().playerB = this.transform.gameObject;
			health_disp = GameObject.Find("HealthB");
			playerNumber = 2;
		}

		////////////////////////////// P1 /////////////////////////////
		if (playerNumber == 1) {
			field_space_Local = 7;
			field_space = 7;
		}
		////////////////////////////// P2 /////////////////////////////
		if (playerNumber == 2) {
			field_space_Local = 10;
			field_space = 10;
		}
	}
	public override void OnSyncedInput() {
		TrueSyncInput.SetInt (INPUT_DIRECTION, requestDirection);
		TrueSyncInput.SetInt (INPUT_BUSTER, requestBuster);
	}


	void Update(){ // Update every game frame
		if (localOwner.Id == owner.Id) { // If player owns this GO
			if(GameObject.Find ("Key Overlay") != null)
				GameObject.Find ("Key Overlay").GetComponent<Key_Listener> ().Navi = this.gameObject;
			if(GameObject.Find ("Buttons") != null)
				GameObject.Find ("Buttons").GetComponent<Tapper> ().Navi = this;
		}

		if(charging) {
			bust_charge += Time.deltaTime;
			if(bust_charge > max_charge) { bust_charge = max_charge; }	// no over charging
		}

		// Update View
		charge_ring.GetComponent<Image>().fillAmount = bust_charge / max_charge;
		if(health_disp != null)
			health_disp.GetComponent<Text>().text = "[HP:" + HP + "] ";


		moveQueueWindow -= Time.deltaTime;
		if (moveQueueWindow <= 0f)
			requestDirection = 0;
		busterQueueWindow -= Time.deltaTime;
		if (busterQueueWindow <= 0f)
			requestBuster = 0;
	}

	public override void OnSyncedUpdate () { // Update every synced frame
		// set the position of the navi equal to the position of the space its on

		int pulledDir = TrueSyncInput.GetInt (INPUT_DIRECTION);
		int pulledBuster = TrueSyncInput.GetInt (INPUT_BUSTER);

		tsTransform.position = new TSVector(field.GetComponent<Field>().spaces[field_space].transform.position.x,field.GetComponent<Field>().spaces[field_space].transform.position.y+0.1f, field.GetComponent<Field>().spaces[field_space].transform.position.z);
		
		moveCooldown -= TrueSyncManager.DeltaTime;
		busterCooldown -= TrueSyncManager.DeltaTime;

		if (pulledBuster > 0 ) {
			if (pendingMoveUp == false && pendingMoveDown == false && pendingMoveLeft == false && pendingMoveRight == false) {
				if(busterCooldown <= 0f && moveCooldown <= 0f) {
					anim.SetTrigger("Shoot");
					if(pulledBuster == 1) {	// uncharged shot
						shot_handler.GetComponent<Shot_Handler>().check_bust(bust_dmg, playerNumber);
					}
					else if(pulledBuster == 2) {	// charged shot
						shot_handler.GetComponent<Shot_Handler>().check_bust(charge1_dmg, playerNumber);
					}
					busterCooldown = 0.25f;
					moveCooldown = 0.26f;
					GetComponent<AudioSource> ().PlayOneShot (Resources.Load<AudioClip> ("Audio/xbustertrim"));
				}
			}
		}

		if (moveCooldown <= 0f) {
			if (pendingMoveUp) {
				field_space = (field_space < 6) ? field_space : field_space - 6;
				pendingMoveUp = false;
			}
			if (pendingMoveDown) {
				field_space = (field_space > 11) ? field_space : field_space + 6;
				pendingMoveDown = false;
			}
			if (pendingMoveLeft) {
				field_space = (field_space % 6 == 0) ? field_space : field_space - 1;
				pendingMoveLeft = false;
			}
			if (pendingMoveRight) {
				field_space = ((field_space - field.GetComponent<Field> ().front_row) % 6 == 0) ? field_space : field_space + 1;
				pendingMoveRight = false;
			}
			if (pulledDir == 1)
				ServerUp ();
			if (pulledDir == 2)
				ServerDown ();
			if (pulledDir == 3)
				ServerLeft ();
			if (pulledDir == 4)
				ServerRight ();
		}


	}

	public void moveUp() {
		next_space = (field_space < 6) ? field_space : field_space - 6;
		if(next_space != field_space) {
			requestDirection = 1;
			moveQueueWindow = 0.15f;
		}
	}
	public void moveDown() {
		requestDirection = 2;
		moveQueueWindow = 0.15f;
	}
	public void moveLeft() {
		requestDirection = 3;
		moveQueueWindow = 0.15f;
	}
	public void moveRight() {
		requestDirection = 4;
		moveQueueWindow = 0.15f;
	}

	public void ServerUp(){
		anim.SetTrigger ("Move");
		pendingMoveUp = true;
		moveCooldown = 0.26f;
	}
	public void ServerDown(){
		anim.SetTrigger ("Move");
		pendingMoveDown = true;
		moveCooldown = 0.26f;
	}
	public void ServerLeft(){
		anim.SetTrigger ("Move");
		pendingMoveLeft = true;
		moveCooldown = 0.26f;
	}
	public void ServerRight(){
		anim.SetTrigger ("Move");
		pendingMoveRight = true;
		moveCooldown = 0.26f;
	}

	public void bust_shot() {
		charging = true;
		requestBuster = 1;
		busterQueueWindow = 0.15f;
	}

	public void charge_release() {
		charging = false;
		if(bust_charge == max_charge) {
			requestBuster = 2;
			busterQueueWindow = 0.15f;
		}
		bust_charge = 0.0f;
	}
	public void hit(int dmg) {
		HP -= dmg;
	}
}

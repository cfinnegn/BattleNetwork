﻿using System.Collections;
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

	public int field_space;	// location of navi on field

	// buster info
	public float bust_charge = 0.0f;
	public float max_charge = 2.0f; // time in seconds for full charge
	bool charging = false;
	public GameObject charge_ring;
	public int bust_dmg = 1;
	public int charge1_dmg = 10;

	// HP
	public int HP = 100;
	GameObject health_dispA;
	GameObject health_dispB;

	public int playerNumber = 1;
	
	public int combo_color = 0;	// color of last chip used

	public GameObject shot_handler;
	public GameObject deck;

	Animator anim;

	void Awake(){
		anim = GetComponent<Animator> ();
		shot_handler = GameObject.Find("Shot Handler");
		field = GameObject.Find ("Field");
	}
	// Use this for initialization
	public override void OnSyncedStart() {
		if (localOwner.Id == owner.Id) { // If player owns this Navi
			GameObject.Find ("Chip Bay").GetComponent<Chip_Hand> ().navi = this.gameObject;
			charge_ring = GameObject.Find ("charge ring");
			GameObject.Find ("Swiper").GetComponent<Swiper> ().Navi = this;
			GameObject.Find ("Buster Button").GetComponent<Buster> ().Navi = this;
			GameObject.Find ("Chip Bay").GetComponent<Chip_Hand> ().navi = this.gameObject;
		}
		// Setting the player's number for easy acess
		if (owner.Id <= 1) {
			playerNumber = 1;
		}
		if (owner.Id == 2) {
			playerNumber = 2;
		}

		if (localOwner.Id <= 1) { // If I'm Player 1, Set these spawnpoints (LOCAL ONLY!)
			if (playerNumber == 1) {
				shot_handler.GetComponent<Shot_Handler> ().playerA = this.transform.gameObject; // Also invert shot_handler's player refs
				field_space = 7;
			}
			if (playerNumber == 2) {
				field_space = 10;
				shot_handler.GetComponent<Shot_Handler> ().playerB = this.transform.gameObject;
			}
		}
		if (localOwner.Id == 2) { // If I'm Player 2, Switch these spawnpoints (LOCAL ONLY!)
			if (playerNumber == 1) {
				shot_handler.GetComponent<Shot_Handler> ().playerB = this.transform.gameObject;
				field_space = 10;
			}
			if (playerNumber == 2) {
				shot_handler.GetComponent<Shot_Handler> ().playerA = this.transform.gameObject;
				field_space = 7;
			} // If I'm Player 2, Switch tile ownership (LOCAL ONLY!)
			field.GetComponent<Field> ().spaces [0].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [1].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [2].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [3].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [4].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [5].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [6].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [7].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [8].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [9].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [10].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [11].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [12].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [13].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [14].GetComponent<TileStatus> ().owner = 2;
			field.GetComponent<Field> ().spaces [15].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [16].GetComponent<TileStatus> ().owner = 1;
			field.GetComponent<Field> ().spaces [17].GetComponent<TileStatus> ().owner = 1;
		}
		if (localOwner.Id != owner.Id) { // If player DOESN"T own this Navi
			tsTransform.scale = new TSVector (-tsTransform.scale.x, tsTransform.scale.y, tsTransform.scale.z);
			health_dispB = GameObject.Find ("HealthB");
			GameObject.Find ("PlayerIDB").GetComponent<Text>().text = owner.Name;
		}
		if (localOwner.Id == owner.Id) {
			health_dispA = GameObject.Find ("HealthA");
			GameObject.Find ("PlayerIDA").GetComponent<Text>().text = owner.Name;
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
		if(charge_ring != null)
			charge_ring.GetComponent<Image>().fillAmount = bust_charge / max_charge;
		if(health_dispA != null)
			health_dispA.GetComponent<Text>().text = "[HP:" + HP + "] ";
		if(health_dispB != null)
			health_dispB.GetComponent<Text>().text = "[HP:" + HP + "] ";


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

		if (field != null) {
				tsTransform.position = new TSVector (field.GetComponent<Field> ().spaces [field_space].transform.position.x, field.GetComponent<Field> ().spaces [field_space].transform.position.y + 0.1f, field.GetComponent<Field> ().spaces [field_space].transform.position.z);
		}
		
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
			// Finishes the movement
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
			// Starts the Movement (1)
			if (pulledDir == 1)
				StartUp ();
			if (pulledDir == 2)
				StartDown ();
			if (pulledDir == 3)
				StartLeft ();
			if (pulledDir == 4)
				StartRight ();
		}


	}
	// Sends a Direction Input Locally
	public void moveUp() {
		requestDirection = 1;
		moveQueueWindow = 0.15f;
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

	// Starts the Movement (2)
	public void StartUp(){
		if (field_space < 6) // Stops OutOfBounds
			return;
		if (field.GetComponent<Field> ().spaces [field_space - 6].GetComponent<TileStatus> ().owner == playerNumber) {// Checks if player owns that tile
			anim.SetTrigger ("Move");
			pendingMoveUp = true;
			moveCooldown = 0.26f;
		}
	}
	public void StartDown(){
		if (field_space > 11) // Stops OutOfBounds
			return;
		if (field.GetComponent<Field> ().spaces [field_space + 6].GetComponent<TileStatus> ().owner == playerNumber) {// Checks if player owns that tile
			anim.SetTrigger ("Move");
			pendingMoveDown = true;
			moveCooldown = 0.26f;
		}
	}
	public void StartLeft(){
		if (field_space % 6 == 0) // Stops OutOfBounds
			return;
		if (localOwner.Id == owner.Id) { // Owner movement
			if (field.GetComponent<Field> ().spaces [field_space - 1].GetComponent<TileStatus> ().owner == playerNumber) {// Checks if player owns that tile
				anim.SetTrigger ("Move");
				pendingMoveLeft = true;
				moveCooldown = 0.26f;
			}
		}
		if (localOwner.Id != owner.Id) { //Flip for non-owner
			if (field_space == 5 || field_space == 11 ||field_space == 17 ) // If on the right ledge and trying to move right
				return;
			if (field.GetComponent<Field> ().spaces [field_space+1].GetComponent<TileStatus> ().owner == playerNumber) {// Checks if player owns that tile
				anim.SetTrigger ("Move");
				pendingMoveRight = true;
				moveCooldown = 0.26f;
			}
		}
	}
	public void StartRight(){
		if ((field_space - field.GetComponent<Field> ().front_row) % 6 == 0) // Stops OutOfBounds
			return;
		if (localOwner.Id == owner.Id) { // Owner movement
			if(field.GetComponent<Field> ().spaces [field_space+1].GetComponent<TileStatus> ().owner == playerNumber){ // Checks if player owns that tile
			anim.SetTrigger ("Move");
			pendingMoveRight = true;
			moveCooldown = 0.26f;
			}
		}
		if (localOwner.Id != owner.Id) { //Flip for non-owner
			if (field.GetComponent<Field> ().spaces [field_space-1].GetComponent<TileStatus> ().owner == playerNumber) {// Checks if player owns that tile
				anim.SetTrigger ("Move");
				pendingMoveLeft = true;
				moveCooldown = 0.26f;
			}
		}
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

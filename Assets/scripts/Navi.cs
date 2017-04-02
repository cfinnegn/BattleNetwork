using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrueSync;

public class Navi : TrueSyncBehaviour {
	const byte INPUT_field_space = 0;
	public GameObject field;
	public int field_space_Local;
	public int field_space;

	public float bust_charge = 0.0f;
	public float max_charge = 2.0f; // time in seconds for full charge
	bool charging = false;
	public GameObject charge_ring;

	public int playerNumber = 1;

	GameObject p1HP_GO;
	GameObject p2HP_GO;
	FP p1HP = 100;
	FP p2HP = 100;

	void Awake(){
		transform.SetParent (GameObject.Find ("Canvas").transform);
		transform.localScale = new Vector3 (1, 1, 1);
		field = GameObject.Find ("Field");
		charge_ring = GameObject.Find ("charge ring");
		p1HP_GO = GameObject.Find ("HealthA");
		p2HP_GO = GameObject.Find ("HealthB");
	}
	// Use this for initialization
	public override void OnSyncedStart() {
		// Setting References
		if (localOwner.Id == owner.Id) { // If player owns this GO
			GameObject.Find ("Swiper").GetComponent<Swiper> ().Navi = this;
			GameObject.Find ("Buttons").GetComponent<Tapper> ().Navi = this;
			GameObject.Find ("Buster Button").GetComponent<Buster> ().Navi = this;
		}
		// Setting the player's number for easy acess
		if (owner.Id == 1)
			playerNumber = 1;
		if (owner.Id == 2)
			playerNumber = 2;

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
		int fs_input = field_space_Local;

		TrueSyncInput.SetInt(INPUT_field_space, fs_input);
	}
	

	void Update(){ // Update every game frame
		if(charging) {
			bust_charge += Time.deltaTime;
			if(bust_charge > max_charge) { bust_charge = max_charge; }	// no over charging
			charge_ring.GetComponent<Image>().fillAmount = bust_charge / max_charge;
		}
	}

	public override void OnSyncedUpdate () { // Update every synced frame
		// set the position of the navi equal to the position of the space its on
		tsTransform.position = new TSVector(field.GetComponent<Field>().spaces[field_space].transform.position.x,field.GetComponent<Field>().spaces[field_space].transform.position.y, field.GetComponent<Field>().spaces[field_space].transform.position.z);

		p1HP_GO.GetComponent<Text>().text = "[HP:" + p1HP + "] ";
		p2HP_GO.GetComponent<Text>().text = "[HP:" + p2HP + "] ";    //!!!!!! temporary for testing: will change !!!!!!

		field_space = TrueSyncInput.GetInt (INPUT_field_space);
	}

	public void moveUp() {
		field_space_Local = (field_space_Local < 6) ? field_space_Local : field_space_Local - 6;
	}
	public void moveDown() {
		field_space_Local = (field_space_Local > 11) ? field_space_Local : field_space_Local + 6;
	}
	public void moveLeft() {
		// use mod to check for back row
		field_space_Local = (field_space_Local%6 == 0) ? field_space_Local : field_space_Local - 1;
	}
	public void moveRight() {
		// subtract front_row to then use mod to show if max dist from back row
		field_space_Local = ((field_space_Local-field.GetComponent<Field>().front_row)%6 == 0) ? field_space_Local : field_space_Local + 1;
	}

	public void bust_shot() {
		charging = true;
		if(playerNumber == 1)
			p2HP -= 1;		//!!!!!! temporary for testing: will change !!!!!!
		if(playerNumber == 2)
			p1HP -= 1;		//!!!!!! temporary for testing: will change !!!!!!
	}

	public void charge_release() {
		if(bust_charge == max_charge) {
			if(playerNumber == 1)
				p2HP -= 10;		//!!!!!! temporary for testing: will change !!!!!!
			if(playerNumber == 2)
				p1HP -= 10;		//!!!!!! temporary for testing: will change !!!!!!
		}
		charging = false;
		bust_charge = 0.0f;
		charge_ring.GetComponent<Image>().fillAmount = 0.0f;
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Listener : MonoBehaviour {
	public GameObject Chip_hand;
	public Navi Navi;
	public GameObject DrawButton;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("w")) { // movement
			Navi.moveUp();
		}
		if(Input.GetKeyDown("a")) {
			Navi.moveLeft();
		}
		if(Input.GetKeyDown("s")) {
			Navi.moveDown();
		}
		if(Input.GetKeyDown("d")) {
			Navi.moveRight();
		}
		if(Input.GetKeyDown("space")) { // Buster Press
			Navi.bust_shot();
		}
		if(Input.GetKeyUp("space")) {   // Buster Release
			Navi.charge_release();
		}
		if(Input.GetKeyDown("+") || Input.GetKeyDown("=")) {	// Draw Chip Button
			DrawButton.GetComponent<Draw_Button>().Draw_Chip();
		}

		if(Input.GetKeyDown("left shift") || Input.GetKeyDown("right shift")) { // Navi Power
			Navi.NaviPowerInput();
		}

		if(Input.GetKeyDown("0")) { // Chips in hand
			if (Chip_hand.GetComponent<Chip_Hand>().chips[0] != null) { // holding a 0 index chip
				Chip_hand.GetComponent<Chip_Hand>().chips[0].GetComponent<BattleChip>().clicked();
			}
		}
		if(Input.GetKeyDown("9")) {
			if(Chip_hand.GetComponent<Chip_Hand>().chips[1] != null) { // holding a 1 index chip
				Chip_hand.GetComponent<Chip_Hand>().chips[1].GetComponent<BattleChip>().clicked();
			}
		}
		if(Input.GetKeyDown("8")) {
			if(Chip_hand.GetComponent<Chip_Hand>().chips[2] != null) { // holding a 2 index chip
				Chip_hand.GetComponent<Chip_Hand>().chips[2].GetComponent<BattleChip>().clicked();
			}
		}
		if(Input.GetKeyDown("7")) {
			if(Chip_hand.GetComponent<Chip_Hand>().chips[3] != null) { // holding a 3 index chip
				Chip_hand.GetComponent<Chip_Hand>().chips[3].GetComponent<BattleChip>().clicked();
			}
		}
		if(Input.GetKeyDown("6")) {
			if(Chip_hand.GetComponent<Chip_Hand>().chips[4] != null) { // holding a 4 index chip
				Chip_hand.GetComponent<Chip_Hand>().chips[4].GetComponent<BattleChip>().clicked();
			}
		}
		if(Input.GetKeyDown("5")) {
			if(Chip_hand.GetComponent<Chip_Hand>().chips[5] != null) { // holding a 0 index chip
				Chip_hand.GetComponent<Chip_Hand>().chips[5].GetComponent<BattleChip>().clicked();
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Listener : MonoBehaviour {
	public GameObject Chip_hand;
	public GameObject Navi;
	public GameObject DrawButton;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("w")) { // movement
			Navi.GetComponent<Navi>().moveUp();
		}
		if(Input.GetKeyDown("a")) {
			Navi.GetComponent<Navi>().moveLeft();
		}
		if(Input.GetKeyDown("s")) {
			Navi.GetComponent<Navi>().moveDown();
		}
		if(Input.GetKeyDown("d")) {
			Navi.GetComponent<Navi>().moveRight();
		}
		if(Input.GetKeyDown("space")) { // Buster Press
			Navi.GetComponent<Navi>().bust_shot();
		}
		if(Input.GetKeyUp("space")) {   // Buster Release
			Navi.GetComponent<Navi>().charge_release();
		}
		if(Input.GetKeyDown("+") || Input.GetKeyDown("=")) {	// Draw Chip Button
			DrawButton.GetComponent<Draw_Button>().Draw_Chip();
		}


		if(Input.GetKeyDown("0")) { // Chips in hand
			GameObject.Find("Chip Bay").GetComponent<Chip_Hand>().chips[0].GetComponent<BattleChip>().clicked();
		}
		if(Input.GetKeyDown("9")) {
			GameObject.Find("Chip Bay").GetComponent<Chip_Hand>().chips[1].GetComponent<BattleChip>().clicked();

		}
		if(Input.GetKeyDown("8")) {
			GameObject.Find("Chip Bay").GetComponent<Chip_Hand>().chips[2].GetComponent<BattleChip>().clicked();

		}
		if(Input.GetKeyDown("7")) {
			GameObject.Find("Chip Bay").GetComponent<Chip_Hand>().chips[3].GetComponent<BattleChip>().clicked();

		}
		if(Input.GetKeyDown("6")) {
			GameObject.Find("Chip Bay").GetComponent<Chip_Hand>().chips[4].GetComponent<BattleChip>().clicked();

		}
		if(Input.GetKeyDown("5")) {
			GameObject.Find("Chip Bay").GetComponent<Chip_Hand>().chips[5].GetComponent<BattleChip>().clicked();

		}
	}
}

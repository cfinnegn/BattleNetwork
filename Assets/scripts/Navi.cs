using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navi : MonoBehaviour {

	public GameObject field;
	public int field_space;
	public GameObject shot_handler;

	public float bust_charge = 0.0f;
	public float max_charge = 1.5f; // time in seconds for full charge
	bool charging = false;
	public GameObject charge_ring;

	public int combo_color = 0;

	// Use this for initialization
	void Start () {
		field_space = 7;
	}
	

	// Update is called once per frame
	void Update () {
		// set the position of the navi equal to the position of the space its on
		transform.position = field.GetComponent<Field>().spaces[field_space].transform.position;

		if(charging) {
			bust_charge += Time.deltaTime;
			if(bust_charge > max_charge) { bust_charge = max_charge; }	// no over charging
			charge_ring.GetComponent<Image>().fillAmount = bust_charge / max_charge;
		}
	}

	public void moveUp() {
		field_space = (field_space < 6) ? field_space : field_space - 6;
	}
	public void moveDown() {
		field_space = (field_space > 11) ? field_space : field_space + 6;
	}
	public void moveLeft() {
		// use mod to check for back row
		field_space = (field_space%6 == 0) ? field_space : field_space - 1;
	}
	public void moveRight() {
		// subtract front_row to then use mod to show if max dist from back row
		field_space = ((field_space-field.GetComponent<Field>().front_row)%6 == 0) ? field_space : field_space + 1;
	}

	public void bust_shot() {
		charging = true;
		shot_handler.GetComponent<Shot_Handler>().check_hitB(1);
	}

	public void charge_release() {
		if(bust_charge == max_charge) {
			shot_handler.GetComponent<Shot_Handler>().check_hitB(10);
		}
		charging = false;
		bust_charge = 0.0f;
		charge_ring.GetComponent<Image>().fillAmount = 0.0f;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navi : MonoBehaviour {

	public GameObject field;
	public int field_space;

	public float bust_charge = 0.0f;
	public float max_charge = 2.0f; // time in seconds for full charge
	bool charging = false;
	public GameObject charge_ring;

	// Enemy HP data held here temporarily for testing purposes only!!!
	public GameObject VS_health;
	int vs_hp = 100;
	// end enemy health data


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

		VS_health.GetComponent<Text>().text = "[HP:" + vs_hp + "] ";    //!!!!!! temporary for testing: will change !!!!!!
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
		vs_hp--;		//!!!!!! temporary for testing: will change !!!!!!
	}

	public void charge_release() {
		if(bust_charge == max_charge) {
			vs_hp -= 10;
		}
		charging = false;
		bust_charge = 0.0f;
		charge_ring.GetComponent<Image>().fillAmount = 0.0f;
	}


}
